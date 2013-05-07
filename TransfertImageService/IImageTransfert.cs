using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace TransfertImageService
{
    [ServiceContract]
    public interface IImageTransfert
    {
        [OperationContract]
        void UploadImage(ImageUploadRequest data);
        [OperationContract]
        ImageDownloadResponse DownloadImage(ImageDownloadRequest data);
        [OperationContract]
        List<String> getAlbumPhotos(String albumID);
        [OperationContract]
        void DelectImage(String imageID);
        [OperationContract]
        void DelectAlbum(String albumID);
        [OperationContract]
        void addUser(String userName, String password);
        [OperationContract]
        void createNewAlbum(String albumID, String userName);
        [OperationContract]
        List<String> getUserAlbum(String userName);
        [OperationContract]
        void disallowUserAlbum(String albumID, String userName);
        [OperationContract]
        void deleteUser(String userName);
        [OperationContract]
        List<String> showUsers();
        [OperationContract]
        List<String> showAlbums();
        [OperationContract]
        Boolean checkLogin(String userName, String password);
    }
    [MessageContract]
    public class ImageUploadRequest
    {
        [MessageHeader(MustUnderstand = true)]
        public ImageInfo ImageInfo;
        [MessageBodyMember(Order = 1)]
        public Stream ImageData;
    }
    [MessageContract]
    public class ImageDownloadResponse
    {
        [MessageBodyMember(Order = 1)]
        public Stream ImageData;
    }
    [MessageContract]
    public class ImageDownloadRequest
    {
        [MessageBodyMember(Order = 1)]
        public ImageInfo ImageInfo;
    }
    [DataContract]
    public class ImageInfo
    {
        [DataMember(Order = 1, IsRequired = true)]
        public string imageID { get; set; }
        [DataMember(Order = 2, IsRequired = true)]
        public string AlbumID { get; set; }
    }
}