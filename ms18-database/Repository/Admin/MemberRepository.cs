using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Admin;
using Maasgroep.SharedKernel.DataModels.Admin;
using System.Security.Cryptography;
using System.Text;

namespace Maasgroep.Database.Admin
{
    public class MemberRepository : EditableRepository<Member, MemberModel, MemberData, MemberHistory>, IMemberRepository
    {
        protected Dictionary<long, string[]> MemberPermissions;
        protected Dictionary<long, string[]> AddedMemberPermissions;
        protected Dictionary<long, string[]> RemovedMemberPermissions;

        public MemberRepository(MaasgroepContext db) : base(db)
        {
            MemberPermissions = new();
            AddedMemberPermissions = new();
            RemovedMemberPermissions = new();
        }

        /** Create MemberModel from Member record */
        public override MemberModel GetModel(Member member)
        {
            return new MemberModel() {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email ?? "",
                Color = member.Color,
                Permissions = GetPermissions(member.Id),
            };
        }

        /** Create or update Member record from data model */
        public override Member? GetRecord(MemberData data, Member? existingMember = null)
        {
            var member = existingMember ?? new();
            member.Name = data.Name;
            member.Color = data.Color;
            var credentials = data as MemberCredentialsData;
            if (credentials?.NewEmail != null)
                member.Email = credentials.NewEmail;
            if (credentials?.NewPassword != null && credentials?.NewPassword != "")
                member.Password = GetPasswordHash(credentials!.NewPassword);
            if (credentials?.NewPermissions != null)
                ChangePermissions(member.Id, credentials!.NewPermissions);
            return member;
        }

        /** Get member by e-mail/password */
        public MemberModel? GetByEmail(string email, string password)
        {
            var member = Db.Member.FirstOrDefault(item => item.Email == email && item.DateTimeDeleted == null);
            if (member == null || member.Password == null || !CheckPassword(password, member.Password))
                return null;
            return GetModel(member);
        }

        /** Get a hash for a password string */
        public string GetPasswordHash(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash);
        }

        /** Check if password is correct for hash */
        public bool CheckPassword(string password, string hash)
            => hash == GetPasswordHash(password);

        /** Check if password is correct for Member ID */
        public bool CheckPassword(string password, long memberId)
        {
            var passwordHash = Db.Member.Where(item => item.Id == memberId && item.DateTimeDeleted == null).Select(item => item.Password).FirstOrDefault();
            return passwordHash != null && CheckPassword(password, passwordHash);
        }

        /** Get all member permissions */
        public List<string> GetPermissions(long memberId)
        {
            var permissions = new Dictionary<string, bool>();
            if (!MemberPermissions.ContainsKey(memberId))
            {
                MemberPermissions[memberId] = Db.MemberPermission
                    .Where(mp => mp.MemberId == memberId && mp.DateTimeDeleted == null)
                    .Join(Db.Permission, mp => mp.PermissionId, p => p.Id, (mp, p) => p.Name)
                    .ToArray() ?? Array.Empty<string>();
            }

            foreach (var permission in MemberPermissions[memberId])
            {
                // Make sure members with e.g. "basePermission.subPermission" also have "basePermission" set
                var n = 1;
                var splitPermission = permission.Split(".");
                while (n <= splitPermission.Length)
                {
                    permissions.TryAdd(String.Join('.', splitPermission.Take(n)), true);
                    n++;
                }
            }
            return permissions.Select(p => p.Key).ToList();
        }

        /** Prepare new permissions for saving to the database */
        public void ChangePermissions(long memberId, string[] permissions)
        {
            GetPermissions(memberId);
            AddedMemberPermissions[memberId] = permissions.Where(p => !MemberPermissions[memberId].Contains(p)).ToArray();
            RemovedMemberPermissions[memberId] = MemberPermissions[memberId].Where(p => !permissions.Contains(p)).ToArray();
            MemberPermissions[memberId] = permissions;
        }

        /** Create a ProductHistory record from a Product record */
        public override MemberHistory GetHistory(Member member)
        {
            GetPermissions(member.Id);
            return new MemberHistory() {
                MemberId = member.Id,
                Name = member.Name,
                Color = member.Color,
                Email = member.Email,
                MemberPermissions = String.Join('|', MemberPermissions[member.Id]),
            };
        }

        /** Update permissions when saving member */
        public override Action<MaasgroepContext> GetSaveAction(Member record)
        {
            var saveAction = base.GetSaveAction(record);
            return (MaasgroepContext db) => {
                saveAction.Invoke(db);
                var memoizedId = record.Id; // Zero for new member
                if (record.Id == 0)
                    db.SaveChanges(); // Member did not exist yet; save it first so it gets an ID
                if (AddedMemberPermissions.ContainsKey(memoizedId))
                {
                    // Permissions were added: find their IDs in the database 
                    var addedPermissions = Db.Permission
                        .Where(p => AddedMemberPermissions[memoizedId].Contains(p.Name))
                        .Select(p => p.Id)
                        .ToArray();

                    // Find any permissions that already exist in the database (previously deleted)
                    var memberPermissionsToUpdate = Db.MemberPermission
                        .Where(mp => mp.MemberId == record.Id && addedPermissions.Contains(mp.PermissionId))
                        .ToDictionary(mp => mp.PermissionId);
                    
                    foreach (var permissionId in addedPermissions)
                    {
                        if (memberPermissionsToUpdate.ContainsKey(permissionId))
                        {
                            // This one already existed (previously deleted)
                            var memberPermission = memberPermissionsToUpdate[permissionId];
                            memberPermission.DateTimeDeleted = null;
                            memberPermission.DateTimeModified = DateTime.UtcNow;
                            memberPermission.MemberModifiedId = record.MemberModifiedId ?? record.MemberCreatedId;
                            Db.MemberPermission.Update(memberPermission);
                            continue;
                        }
                        // Create the new permission
                        var newPermission = new MemberPermission()
                        {
                            PermissionId = permissionId,
                            MemberId = record.Id,
                            MemberCreatedId = record.MemberModifiedId ?? record.MemberCreatedId,
                        };
                        Db.MemberPermission.Add(newPermission);
                    }
                    AddedMemberPermissions.Remove(memoizedId);
                }
                if (RemovedMemberPermissions.ContainsKey(memoizedId))
                {
                    // Permissions were removed
                    var removedPermissions = Db.Permission
                        .Where(p => RemovedMemberPermissions[memoizedId].Contains(p.Name))
                        .Select(p => p.Id)
                        .ToArray();

                    // Find the permissions that exist in the database
                    var memberPermissionsToUpdate = Db.MemberPermission
                        .Where(mp => mp.MemberId == record.Id && removedPermissions.Contains(mp.PermissionId))
                        .ToArray();

                    foreach (var removedPermission in memberPermissionsToUpdate)
                    {
                        // Mark the removed permission as deleted
                        removedPermission.DateTimeDeleted = DateTime.UtcNow;
                        removedPermission.MemberDeletedId = record.MemberModifiedId ?? record.MemberCreatedId;
                        Db.MemberPermission.Update(removedPermission);
                    }

                    RemovedMemberPermissions.Remove(memoizedId);
                }
                if (MemberPermissions.ContainsKey(memoizedId))
                    MemberPermissions.Remove(memoizedId);
            };
        }
    }
}
