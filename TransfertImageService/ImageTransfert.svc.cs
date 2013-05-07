using System;
using System.IO;
using OperationsDeBD;
using System.Collections.Generic;


namespace TransfertImageService
{
    public class ImageTransfert : IImageTransfert
    {
        private Operations bdAccess = new Operations();
        public void UploadImage(ImageUploadRequest data)
        {
            // Stocker l’image en BDD
            byte[] imageBytes = null;
            MemoryStream imageStreamEnMemoire = new MemoryStream();
            data.ImageData.CopyTo(imageStreamEnMemoire);
            imageBytes = imageStreamEnMemoire.ToArray();
            bdAccess.addImage(data.ImageInfo.imageID,imageBytes,data.ImageInfo.AlbumID);
            imageStreamEnMemoire.Close();
            data.ImageData.Close();
        }

        public ImageDownloadResponse DownloadImage(ImageDownloadRequest data)
        {
            // Récupérer l'image stockée en BDD et la transférer au client
            byte[] imageBytes = bdAccess.getImage(data.ImageInfo.imageID);
            MemoryStream imageStreamEnMemoire = new MemoryStream(imageBytes);
            ImageDownloadResponse response = new ImageDownloadResponse();
            response.ImageData = imageStreamEnMemoire;
            return response;
        }

        public List<String> getAlbumPhotos(String albumID) 
        {
            return bdAccess.getAlbumPhotos(albumID);
        }

        public void DelectImage(String imageID)
        {
            bdAccess.deleteImage(imageID);
        }

        public void DelectAlbum(String albumID)
        {
            bdAccess.deleteAlbum(albumID);
        }

        public void addUser(String userName, String password) 
        {
            bdAccess.addUser(userName, password);
        }
        public void createNewAlbum(String albumID, String userName) 
        {
            bdAccess.createNewAlbum(albumID, userName);
        }

        public List<String> getUserAlbum(String userName) 
        {
            return bdAccess.getUserAlbum(userName);
        }

        public void disallowUserAlbum(String albumID, String userName) 
        {
            bdAccess.disallowUserAlbum(albumID, userName);
        }

        public void deleteUser(String userName) 
        {
            bdAccess.deleteUser(userName);
        }

        public List<String> showUsers()
        {
            return bdAccess.showUsers();
        }

        public List<String> showAlbums()
        {
            return bdAccess.showAlbums();
        }

        public Boolean checkLogin(String userName, String password) 
        {
            return bdAccess.checkLogin(userName, password);
        }
    }
}