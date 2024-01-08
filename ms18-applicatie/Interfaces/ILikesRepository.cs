﻿using Maasgroep.Database.Context.Tables.PhotoAlbum;
using ms18_applicatie.Models.team_d;

namespace ms18_applicatie.Interfaces;

public interface ILikesRepository
{
    Task<Guid?> AddLike(Like like);
    Task<Like?> GetLike(Guid photoId, long userId);
    Task DeleteLike(Guid likeId);
    Task<IEnumerable<LikeViewModel>> GetAllLikesForPhoto(Guid photoId);
    Task<IEnumerable<PhotoViewModel>> GetTopLikedPhotos(DateTime startDate, DateTime endDate, int topCount);
}

