using Maasgroep.Database.Admin;
using Maasgroep.Database.Context.Tables.PhotoAlbum;
using Microsoft.EntityFrameworkCore;
using ms18_applicatie.Repository.PhotoAlbum;

namespace ms18_applicatie_test.team_d.Repository;

[TestFixture]
public class LikesRepositoryTests
{
    private async Task<(MaasgroepContext, List<Guid>, List<long>)> InitializeContextWithData()
    {
        var context = TestDbContextFactory.Create();
        var validPhotoIds = new List<Guid>();
        var validMemberIds = new List<long>();

        for (var i = 0; i < 5; i++)
        {
            var photo = new Photo
            {
                UploaderId = i,
                ContentType = "image/jpeg",
                UploadDate = DateTime.Now,
                Title = $"Photo {i}",
                ImageData = new byte[] { 1, 2, 3, 4, 5 },
                TakenOn = DateTime.Now.AddDays(-i),
                NeedsApproval = false,
                AlbumLocationId = null,
            };
            context.Photos.Add(photo);

            var member = new Member
            {
                Id = i + 1,
                Name = $"Member {i}",
                Email = $"Member{i}@example.com",
                IsGuest = false
            };
            context.Member.Add(member);
        }

        await context.SaveChangesAsync();

        validPhotoIds.AddRange(context.Photos.Select(p => p.Id));
        validMemberIds.AddRange(context.Member.Select(m => m.Id));
        return (context, validPhotoIds, validMemberIds);
    }

    [Test]
    public async Task AddLike_ValidLike_AddsAndReturnsLikeId()
    {
        var (context, validPhotoIds, validMemberIds) = await InitializeContextWithData();
        var repository = new LikesRepository(context);
        var like = new Like
        {
            PhotoId = validPhotoIds[0],
            MemberId = validMemberIds[0],
            LikedOn = DateTime.UtcNow
        };

        var result = await repository.AddLike(like);
        var likeResult = await context.Likes.FindAsync(result);

        Assert.That(result, Is.Not.Null);
        Assert.That(likeResult, Is.Not.Null, "Like result should not be null");
        Assert.That(likeResult?.PhotoId, Is.EqualTo(like.PhotoId), "PhotoId should match");
        Assert.That(likeResult?.MemberId, Is.EqualTo(like.MemberId), "MemberId should match");
        Assert.That(likeResult?.LikedOn, Is.EqualTo(like.LikedOn).Within(TimeSpan.FromSeconds(1)), "LikedOn should match within 1 second");
        
        await context.DisposeAsync();
    }

    [Test]
    public async Task DeleteLike_ExistingLike_RemovesLike()
    {
        var (context, validPhotoIds, validMemberIds) = await InitializeContextWithData();
        var repository = new LikesRepository(context);
        var like = new Like
        {
            PhotoId = validPhotoIds[1],
            MemberId = validMemberIds[1],
            LikedOn = DateTime.UtcNow
        };

        var likeId = await repository.AddLike(like);
        await repository.DeleteLike(likeId.Value);

        var deletedLike = await context.Likes.FindAsync(likeId.Value);

        Assert.That(likeId, Is.Not.EqualTo(null));
        Assert.That(deletedLike, Is.Null);
        await context.DisposeAsync();
    }

    [Test]
    public async Task GetAllLikesForPhoto_WithMultipleLikes_ReturnsAllLikes()
    {
        var (context, validPhotoIds, validMemberIds) = await InitializeContextWithData();
        var repository = new LikesRepository(context);

        for (var i = 0; i < 3; i++)
        {
            var like = new Like
            {
                PhotoId = validPhotoIds[0],
                MemberId = validMemberIds[i],
                LikedOn = DateTime.UtcNow.AddDays(-i)
            };
            await repository.AddLike(like);
        }

        var likes = await repository.GetAllLikesForPhoto(validPhotoIds[0]);

        Assert.That(likes, Is.Not.Null);
        Assert.That(likes, Has.Count.EqualTo(3));
        foreach (var like in likes)
        {
            Assert.That(like.PhotoId, Is.EqualTo(validPhotoIds[0]));
            Assert.That(validMemberIds, Does.Contain(like.MemberId));
        }
        await context.DisposeAsync();
    }

    [Test]
    public async Task GetLike_WithExistingLike_ReturnsCorrectLike()
    {
        var (context, validPhotoIds, validMemberIds) = await InitializeContextWithData();
        var repository = new LikesRepository(context);

        var testPhotoId = validPhotoIds[0];
        var testMemberId = validMemberIds[0];

        var like = new Like
        {
            PhotoId = testPhotoId,
            MemberId = testMemberId,
            LikedOn = DateTime.UtcNow
        };
        await repository.AddLike(like);

        var retrievedLike = await repository.GetLike(testPhotoId, testMemberId);

        Assert.That(retrievedLike, Is.Not.Null);
        Assert.That(retrievedLike?.PhotoId, Is.EqualTo(testPhotoId));
        Assert.That(retrievedLike?.MemberId, Is.EqualTo(testMemberId));
        
        await context.DisposeAsync();
    }

    [Test]
    public async Task GetLike_WithNonExistingLike_ReturnsNull()
    {
        var (context, _, _) = await InitializeContextWithData();
        var repository = new LikesRepository(context);

        var nonExistingPhotoId = Guid.NewGuid();
        var nonExistingMemberId = 999L; 

        var retrievedLike = await repository.GetLike(nonExistingPhotoId, nonExistingMemberId);

        Assert.That(retrievedLike, Is.Null);

        await context.DisposeAsync();
    }

    [Test]
    public async Task GetTopLikedPhotos_ReturnsTopPhotosBasedOnLikes()
    {
        var (context, validPhotoIds, validMemberIds) = await InitializeContextWithData();
        var repository = new LikesRepository(context);

        var startDate = DateTime.UtcNow.AddDays(-10);
        var endDate = DateTime.UtcNow;

        var expectedDates = new List<DateTime>();
        for (var i = 0; i < 5; i++)
        {
            for (var j = 0; j <= i; j++)
            {
                var like = new Like
                {
                    PhotoId = validPhotoIds[i],
                    MemberId = validMemberIds[j % validMemberIds.Count],
                    LikedOn = DateTime.UtcNow.AddDays(-j)
                };
                await repository.AddLike(like);
            }
        }

        var topCount = 3;

        var topLikedPhotos = await repository.GetTopLikedPhotos(startDate, endDate, topCount);

        Assert.That(topLikedPhotos, Is.Not.Null);
        Assert.That(topLikedPhotos.Count, Is.EqualTo(topCount));


        var expectedOrder = topLikedPhotos.OrderByDescending(p => p.LikesCount).Select(p => p.Id).ToList();
        var actualOrder = topLikedPhotos.Select(p => p.Id).ToList();
        Assert.That(actualOrder, Is.EqualTo(expectedOrder));


        await context.DisposeAsync();
    }
}

