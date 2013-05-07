using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace WebClient
{
    public partial class VoirImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void valid_Click(object sender, EventArgs e)
        {
            WebServeur.ImageTransfertClient client = new WebServeur.ImageTransfertClient();
            String[] photos = client.getAlbumPhotos(albumList.SelectedValue);
            for (int i = 0; i < photos.Length; i++)
            {   
                System.Web.UI.WebControls.ImageButton img = new System.Web.UI.WebControls.ImageButton();
                Panel1.Controls.Add(img);
                img.ImageUrl = "Image.aspx?ImageID=" + photos[i];
                img.Width = 200;
                img.Height = 150;
                img.ID = photos[i];
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            String user;
            WebServeur.ImageTransfertClient client = new WebServeur.ImageTransfertClient();
            if(client.checkLogin((user=userName.Text),password.Text))
            {   
                resultat.Text = String.Format("Welcome,{0}!You have these albums.You can choose the album to show it.", user);
                Label2.Visible = false;
                userName.Visible = false;
                Label3.Visible = false;
                password.Visible = false;
                submit.Visible = false;
                String[] albums = client.getUserAlbum(user);
                for (int i = 0; i < albums.Length; i++)
                {
                    System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                    albumList.Items.Add(item);
                    item.Text = albums[i];
                }
            }
            else
            {
                resultat.Text = String.Format("user doesn't exist or password isn't correct.");
            }
        }
    }
}