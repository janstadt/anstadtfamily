using AutoMapper;
using photoshare.Models;
using System.Linq;
using System;
using photoshare.Models.Enums;
namespace vdz.ca.Mappers
{
    public static class Mappers
    {
        public static void Register()
        {
            Mapper.CreateMap<LoginModel, LoginHeaderModel>();
            Mapper.CreateMap<photo, PhotoModel>().ReverseMap();
            Mapper.CreateMap<photo, PhotoEntity>()//.ForMember(
            //    dest => dest.Favorite,
            //    opt => opt.MapFrom(src => src.favoritephotos.Any(x => x.PhotoId == src.Id && x.Owner == src.Owner))
            //)
            .ForMember(
                dest => dest.Comments,
                opt => opt.MapFrom(src => src.comments.Where(x => x.PhotoId == src.Id).ToList())
            ).ReverseMap().ForMember(
                dest => dest.user,
                opt => opt.Ignore()
            ).ForMember(
                dest => dest.favoritephotos,
                opt => opt.Ignore()
            ).ForMember(
                dest => dest.comments,
                opt => opt.Ignore()
            );
            Mapper.CreateMap<PhotoModel, PhotoEntity>().ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Id) ? Guid.NewGuid() : new Guid(src.Id))
            ).ReverseMap().ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid().ToString() : src.Id.ToString())
            );
            Mapper.CreateMap<photoalbum, PhotoAlbumModel>()
                .ForMember(
                    dest => dest.Clean,
                    opt => opt.MapFrom(src => src.Title.ToLowerInvariant().Replace(" ", "-"))
            ).ReverseMap();
            Mapper.CreateMap<tag, TagEntity>().ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id)
            ).ReverseMap();
            Mapper.CreateMap<TagEntity, TagModel>().ReverseMap().ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id)
            );
            Mapper.CreateMap<comment, CommentModel>().ReverseMap();
            Mapper.CreateMap<PhotoEntity, favoritephoto>().ForMember(
                dest => dest.PhotoId,
                opt => opt.MapFrom(src => src.Id)
            ).ForMember(
                dest => dest.user,
                opt => opt.Ignore()
            ).ForMember(
                dest => dest.photo,
                opt => opt.Ignore()
            ).ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => Guid.NewGuid())
            );
            Mapper.CreateMap<favoritealbum, FavoriteAlbumEntity>().ReverseMap();
            Mapper.CreateMap<FavoriteAlbumEntity, FavoriteAlbumModel>().ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id.ToString())
            ).ReverseMap().ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => new Guid(src.Id))
            );
            Mapper.CreateMap<favoritephoto, FavoritePhotoModel>().ReverseMap();
            Mapper.CreateMap<UserEntity, UserModel>().ReverseMap();
            Mapper.CreateMap<UserEntity, UserPhotoAlbumsModel>().ReverseMap();
            Mapper.CreateMap<UserModelBase, UserModel>().ReverseMap();
            Mapper.CreateMap<UserModelBase, UserEntity>().ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Id) ? Guid.NewGuid() : new Guid(src.Id))
            ).ReverseMap();
            Mapper.CreateMap<user, UserEntity>().ForMember(
                dest => dest.FavoritePhotos,
                opt => opt.MapFrom(src => src.favoritephotos.ToList())
            ).ForMember(
                dest => dest.PhotoAlbums,
                opt => opt.MapFrom(src => src.photoalbums.ToList())
            );
            Mapper.CreateMap<UserEntity, user>().ForMember(
                dest => dest.photoalbums, 
                opt => opt.Ignore()
            ).ForMember(
                dest => dest.photos,
                opt => opt.Ignore()
            ).ForMember(
                dest => dest.favoritealbums,
                opt => opt.Ignore()
            ).ForMember(
                dest => dest.favoritephotos,
                opt => opt.Ignore()
            );
            Mapper.CreateMap<UserModel, LoginModel>().ReverseMap();
            Mapper.CreateMap<PhotoAlbumModel, PhotoAlbumEntity>().ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Id) ? Guid.NewGuid() : new Guid(src.Id))
            ).ReverseMap().ForMember(
                    dest => dest.Clean,
                    opt => opt.MapFrom(src => src.Title.ToLowerInvariant().Replace(" ", "-"))
            );
            Mapper.CreateMap<PhotoAlbumEntity, photoalbum>().ForMember(
                    dest => dest.photos,
                    opt => opt.Ignore()
            ).ForMember(
                    dest => dest.favoritealbums,
                    opt => opt.Ignore()
            ).ReverseMap();
            Mapper.CreateMap<PhotoAlbumEntity, favoritealbum>().ForMember(
                    dest => dest.photoalbum,
                    opt => opt.Ignore()
            ).ForMember(
                    dest => dest.user,
                    opt => opt.Ignore()
            ).ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => Guid.NewGuid())
            ).ForMember(
                    dest => dest.AlbumId,
                    opt => opt.MapFrom(src => src.Id)
            );
            Mapper.CreateMap<photoalbum, favoritealbum>().ForAllMembers(dest => dest.Ignore());
            Mapper.CreateMap<photoalbum, favoritealbum>().ForMember(
                dest => dest.AlbumId,
                opt => opt.MapFrom(src => src.Id)
            ).ForMember(
                dest => dest.Date,
                opt => opt.MapFrom(src => src.Date)
            ).ForMember(
                dest => dest.Owner,
                opt => opt.MapFrom(src => src.Owner)
            ).AfterMap((src, dest) => {
                dest.Id = dest.Id == Guid.Empty ? Guid.NewGuid() : dest.Id;
            });
            Mapper.CreateMap<photo, FavoritePhotoEntity>().ForAllMembers(dest => dest.Ignore());
            Mapper.CreateMap<photo, FavoritePhotoEntity>().ForMember(
                dest => dest.PhotoId,
                opt => opt.MapFrom(src => src.Id)
            );
            Mapper.CreateMap<photo, favoritephoto>().ForAllMembers(dest => dest.Ignore());
            Mapper.CreateMap<photo, favoritephoto>().ForMember(
                dest => dest.PhotoId,
                opt => opt.MapFrom(src => src.Id)
            ).ForMember(
                dest => dest.Date,
                opt => opt.MapFrom(src => src.Date)
            ).ForMember(
                dest => dest.Owner,
                opt => opt.MapFrom(src => src.Owner)
            ).AfterMap((src, dest) =>
            {
                dest.Id = dest.Id == Guid.Empty ? Guid.NewGuid() : dest.Id;
            });
            Mapper.CreateMap<PhotoAlbumEntity, TagEntity>().ForAllMembers(dest => dest.Ignore());
            Mapper.CreateMap<PhotoAlbumEntity, TagEntity>().ForMember(
                dest => dest.ParentId,
                opt => opt.MapFrom(src => src.Id)
            ).ForMember(
                dest => dest.Type,
                opt => opt.UseValue(TagType.Albums.ToString())
            );
            Mapper.CreateMap<string, TagEntity>().ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src)
            );
        }
    }
}
