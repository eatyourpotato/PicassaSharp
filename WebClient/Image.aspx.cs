using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace WebClient
{
    public partial class Image : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String id = Request.QueryString["ImageID"];
            // Si ce paramètre n'est pas nul
            if (id != null)
            {
                var client = new WebServeur.ImageTransfertClient();

                // on récupére notre image là où il faut
                WebServeur.ImageInfo imageInfo = new WebServeur.ImageInfo();
                imageInfo.imageID = id;
                Stream stream = client.DownloadImage(imageInfo);
                MemoryStream imageStreamEnMemoire = new MemoryStream();
                stream.CopyTo(imageStreamEnMemoire);
                Byte[] bytes = imageStreamEnMemoire.ToArray();

                // et on crée le contenu de notre réponse à la requête HTTP
                // (ici un contenu de type image)
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "image/jpeg";
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
        }
    }
}