using System.Text.Json.Serialization;
using Ms18.Database.Models.TeamC.Admin;

namespace Ms18.Database.Repository.TeamC.ViewModel
{
    public class MemberViewModel
    {
        
        public long? ID { get; set; }
        public string Name { get; set; }

        public MemberViewModel(Member dbRec)
        {
            ID = dbRec.Id;
            Name = dbRec.Name;
        }
        
        [JsonConstructor]
        public MemberViewModel() { }
        
    }
}
