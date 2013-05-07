using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace OperationsDeBD
{
    public class Operations
    {
        static String connectionStr = "Server=X109;Database=projet;Integrated Security=true;";
        SqlConnection connexion = new SqlConnection(connectionStr);
        static Char[] trimChars = {' '};

        public void addImage(String imageID, byte[] image,String albumID)
        {
            try
            {
                // connexion au serveur
                connexion.Open();
                // construit la requête
                SqlCommand ajoutImage = new SqlCommand(
                "INSERT INTO Image (imageID, size, blob,albumID) " +
                "VALUES(@imageID, @size, @blob,@albumID)", connexion);
                ajoutImage.Parameters.Add("@imageID", SqlDbType.VarChar, imageID.Length).Value
                = imageID;
                ajoutImage.Parameters.Add("@size", SqlDbType.Int).Value = image.Length;
                ajoutImage.Parameters.Add("@blob", SqlDbType.Image, image.Length).Value
                = image;
                ajoutImage.Parameters.Add("@albumID", SqlDbType.VarChar, albumID.Length).Value
                = albumID;
                // execution de la requête
                ajoutImage.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur :" + e.Message);
            }
            finally
            {
                // dans tous les cas on ferme la connexion
                connexion.Close();
            }
        }

        public byte[] getImage(String imageID)
        {
            byte[] blob = null;
            try
            {
                // connexion au serveur
                connexion.Open();
                // construit la requête
                SqlCommand getImage = new SqlCommand(
                "SELECT imageID,size, blob,albumID " +
                "FROM Image " +
                "WHERE imageID = @imageID", connexion);
                getImage.Parameters.Add("@imageID", SqlDbType.VarChar, imageID.Length).Value =
                imageID;
                // exécution de la requête et création du reader
                SqlDataReader myReader =
                getImage.ExecuteReader(CommandBehavior.SequentialAccess);
                if (myReader.Read())
                {
                    // lit la taille du blob
                    int size = myReader.GetInt32(1);
                    blob = new byte[size];
                    // récupére le blob de la BDD et le copie dans la variable blob
                    myReader.GetBytes(2, 0, blob, 0, size);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur :" + e.Message);
            }
            finally
            {
                // dans tous les cas on ferme la connexion
                connexion.Close();
            }
            return blob;
        }

        public void deleteImage(String imageID)
        {
            try
            {
                // connexion au serveur
                connexion.Open();
                // construit la requête
                SqlCommand deleteImage = new SqlCommand(
                "DELETE FROM Image " +
                "WHERE imageID = '"+imageID +"'", connexion);
                deleteImage.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur :" + e.Message);
            }
            finally
            {
                // dans tous les cas on ferme la connexion
                connexion.Close();
            }
        }

        public void addUser(String userName, String password) 
        {
            try
            {
                // connexion au serveur
                connexion.Open();
                // construit la requête
                SqlCommand ajoutUser = new SqlCommand(
                "INSERT INTO UserTable (userName, password) " +
                "VALUES('"+ userName + "', '"+ password + "')", connexion);
                // execution de la requête
                ajoutUser.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur :" + e.Message);
            }
            finally
            {
                // dans tous les cas on ferme la connexion
                connexion.Close();
            }
        }

        public void createNewAlbum(String albumID, String userName) 
        {
            try
            {
                // connexion au serveur
                connexion.Open();
                // construit la requête
                SqlCommand createAlbum = new SqlCommand(
                "INSERT INTO Album (albumID,userName ) " +
                "VALUES(@albumID, @userName)", connexion);
                createAlbum.Parameters.Add("@albumID", SqlDbType.VarChar, albumID.Length).Value
                = albumID;
                createAlbum.Parameters.Add("@userName", SqlDbType.VarChar, userName.Length).Value = userName;
                // execution de la requête
                createAlbum.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur :" + e.Message);
            }
            finally
            {
                // dans tous les cas on ferme la connexion
                connexion.Close();
            }
        }

        public List<String> getUserAlbum(String userName) 
        {
            List<String> Album = new List<String>();
            try 
            {
                connexion.Open();
                SqlCommand getUserAlbum = connexion.CreateCommand();
                getUserAlbum.CommandText = "SELECT AlbumID FROM Album WHERE userName = '" + userName + "'";
                SqlDataReader myReader = getUserAlbum.ExecuteReader();
                while (myReader.Read())
                { 
                    Album.Add(myReader["AlbumID"].ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur :" + e.Message);
            }
            finally
            {
                connexion.Close();
            }
            return Album;
        }

        public List<String> getAlbumPhotos(String albumID)
        {
            List<String> photos = new List<String>();
            try
            {
                connexion.Open();
                SqlCommand getAlbumPhotos = connexion.CreateCommand();
                getAlbumPhotos.CommandText = "SELECT imageID FROM Image WHERE albumID = @albumID";
                getAlbumPhotos.Parameters.Add("@albumID", SqlDbType.VarChar, albumID.Length).Value = albumID;
                SqlDataReader myReader = getAlbumPhotos.ExecuteReader();
                while (myReader.Read())
                {
                    photos.Add((Convert.ToString(myReader["imageID"]).Trim(trimChars)));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur :" + e.Message);
            }
            finally
            {
                connexion.Close();
            }
            return photos;
        }

        public void deleteAlbum(String albumID) 
        {
            try
            {
                // connexion au serveur
                connexion.Open();
                // construit la requête
                SqlCommand deleteAlbum = new SqlCommand(
                "DELETE FROM Album " +
                "WHERE albumID = '" + albumID +"'", connexion);
                // exécution de la requête et création du reader
                deleteAlbum.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur :" + e.Message);
            }
            finally
            {
                // dans tous les cas on ferme la connexion
                connexion.Close();
            }
        }

        public void disallowUserAlbum(String albumID, String userName) 
        {
            try
            {
                // connexion au serveur
                connexion.Open();
                // construit la requête
                SqlCommand disallowUserAlbum = new SqlCommand(
                "DELETE FROM Album WHERE albumID = '" + albumID + "' AND userName = '" + userName + "'", connexion);
                disallowUserAlbum.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur :" + e.Message);
            }
            finally
            {
                // dans tous les cas on ferme la connexion
                connexion.Close();
            }
        }

        public void deleteUser(String userName) 
        {
            try
            {
                // connexion au serveur
                connexion.Open();
                // construit la requête
                SqlCommand deleteUser = new SqlCommand(
                "DELETE FROM UserTable " +
                "WHERE userName = '" + userName + "'", connexion);
                // exécution de la requête et création du reader
                deleteUser.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur :" + e.Message);
            }
            finally
            {
                // dans tous les cas on ferme la connexion
                connexion.Close();
            }
        
        }

        public List<String> showUsers() 
        {
            List<String> users = new List<String>();
            try
            {
                connexion.Open();
                SqlCommand getUsers = connexion.CreateCommand();
                getUsers.CommandText = "SELECT userName FROM UserTable";
                                SqlDataReader myReader = getUsers.ExecuteReader();
                while (myReader.Read())
                {
                    users.Add((Convert.ToString(myReader["userName"]).Trim(trimChars)));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur :" + e.Message);
            }
            finally
            {
                connexion.Close();
            }
            return users;
        }

        public List<String> showAlbums() 
        {
            List<String> albums = new List<String>();
            try
            {
                connexion.Open();
                SqlCommand getAlbums = connexion.CreateCommand();
                getAlbums.CommandText = "SELECT albumID,userName FROM Album";
                SqlDataReader myReader = getAlbums.ExecuteReader();
                while (myReader.Read())
                {
                    albums.Add("user: " + (Convert.ToString(myReader["userName"]).Trim(trimChars)) + 
                        " albumID : " + (myReader["albumID"].ToString()).Trim(trimChars));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur :" + e.Message);
            }
            finally
            {
                connexion.Close();
            }
            return albums;
        }

        public Boolean checkLogin(String userName, String password) 
        {
            try
            {
                // connexion au serveur
                connexion.Open();
                // construit la requête
                SqlCommand checkLogin = new SqlCommand(
                "SELECT userName,password FROM UserTable WHERE userName ='" + userName + "'AND password='" + password + "'",                      connexion);
                // execution de la requête
                SqlDataReader reader = checkLogin.ExecuteReader();
                if (reader.Read())
                    return true;
                else 
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur :" + e.Message);
                return false;
            }
            finally
            {
                // dans tous les cas on ferme la connexion
                connexion.Close();
            }
        }
    }
}
