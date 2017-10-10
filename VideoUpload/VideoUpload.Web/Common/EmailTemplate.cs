using System.Text;
using VideoUpload.Web.Models.Identity;

namespace VideoUpload.Web.Common
{
  public class EmailTemplate
  {
    /// <summary>
    /// This will be used by known user of the app
    /// </summary>
    /// <param name="currentUser"></param>
    /// <param name="recipientName"></param>
    /// <param name="message"></param>
    /// <param name="hyperLink"></param>
    /// <returns></returns>
    public static string GetTemplate(AppUser currentUser, string recipientName, string message, string hyperLink, string defaultFontName = "Porsche Next TT")
    {

      var template = new StringBuilder();
      template.Append($"<p style='font:15px { defaultFontName }'>Dear { recipientName },<br/>");
      template.Append($"<p style='font:15px { defaultFontName }'>{ message }<br/>");
      template.Append($"<p style='font:15px { defaultFontName }'><a href=\"{ hyperLink }\">Porsche Visual Reception</a><br/>");
      template.Append($"<div><p style='font:15px { defaultFontName }'>Best Regards,</p>");
      template.Append($"<span style='font:15px { defaultFontName }'><b>{ currentUser.FullName }</b></span><br/>");
      template.Append($"<span style='font:15px { defaultFontName }'>{ currentUser.JobTitle }<span><br/><br/>");
      template.Append($"<span style='font:15px { defaultFontName }'>Porsche Center Doha</span><br/>");
      template.Append($"<span style='font:15px { defaultFontName }'>Alboraq Automobiles Co. w.l.l<span><br/>");
      template.Append($"<span style='font:15px { defaultFontName }'>{ currentUser.WorkAddress }</span><br/><br/>");
      template.Append($"<b>Phone:</b> { currentUser.PhoneNumber }<br/>");
      template.Append($"<b>Direct Line:</b> { currentUser.DirectLine }<br/>");
      template.Append($"<b>Fax:</b> { currentUser.FaxNumber }<br/>");
      return template.ToString();
    }
    /// <summary>
    /// This will be used when unknown user watch the posted video
    /// </summary>
    /// <param name="user"></param>
    /// <param name="recipientName"></param>
    /// <param name="message"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GetTemplate(IdentityUser user, string recipientName, string message, string url, string defaultFontName = "Porsche Next TT")
    {

      var template = new StringBuilder();
      template.Append($"<p style='font:15px { defaultFontName }'>Dear { recipientName },<br/>");
      template.Append($"<p style='font:15px { defaultFontName }'>{ message }<br/>");
      template.Append($"<p style='font:15px { defaultFontName }'>{ url } <br/>");
      //template.Append("<div><p style='font:15px Porsche News Gothic'>Best Regards,</p>");
      //template.Append("<span style='font:15px Porsche News Gothic'><b>" + $"{user.FirstName} {user.LastName}" + "</b></span><br/>");
      //template.Append("<span style='font:15px Porsche News Gothic'>" + user.JobTitle + "<span><br/><br/>");
      //template.Append("<span style='font:15px Porsche News Gothic'>Porsche Center Doha</span><br/>");
      //template.Append("<span style='font:15px Porsche News Gothic'>Alboraq Automobiles Co. w.l.l<span><br/>");
      //template.Append("<span style='font:15px Porsche News Gothic'>" + user.WorkAddress + "</span><br/><br/>");
      //template.Append("<b>Phone:</b> " + user.PhoneNumber + "<br/>");
      //template.Append("<b>Direct Line:</b> " + user.DirectLine + "<br/>");
      //template.Append("<b>Fax:</b> " + user.FaxNumber + "<br/>");
      //template.Append("<b>Email:</b> " + user.Email + "<br/></div>");
      return template.ToString();
    }
  }
}