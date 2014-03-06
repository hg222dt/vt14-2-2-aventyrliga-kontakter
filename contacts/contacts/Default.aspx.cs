using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel.DataAnnotations;
using contacts.Model;


namespace contacts
{
    public partial class Default : System.Web.UI.Page
    {
        private Service _service;

        private Service Service
        {
            get { return _service ?? (_service = new Service()); }
        }

        //Anropas när sidan laddas
        protected void Page_Load(object sender, EventArgs e)
        {
            //Iteration som infaller om sessionen status returnerar "true"
            if (Session["Status"] as bool? == true)
            {
                //Meddelande visas och skrivs ut.
                LabelStatusMessage.Text = Request.QueryString["status"];
                statusMessage.Visible = true;
                LabelStatusMessage.Visible = true;
                Session.Remove("Status");
            }
            else
            {   
                //Meddelande visas inte.
                statusMessage.Visible = false;
                LabelStatusMessage.Visible = false;
            }
        }

        //Metod för att häömta all tillgänglig data ur databasen.
        public IEnumerable<Contact> ContactListView_GetData()
        {
            return Service.GetContacts(); 
        }

        //Metod för att hämta data ur databasen sidovis.
        public IEnumerable<Contact> ContactListView_GetDataPageWise(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            return Service.GetContactsPageWise(maximumRows, startRowIndex, out totalRowCount);
        }

        //Metod som hanterar att lägga till en kontakt.
        public void ContactListView_InsertItem(Contact Contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Service.SaveContact(Contact);
                    Session["Status"] = true;
                    var status = "Kontakten har lagts till!";
                    Response.Redirect("~/Default.aspx?status=" + status);
                }
                catch (Exception ex)
                {
                    var validationResults = ex.Data["ValidationResults"] as IEnumerable<ValidationResult>;

                    if(validationResults != null)
                    {
                        foreach(var validationResult in validationResults)
                        {
                            foreach(var memberName in validationResult.MemberNames)
                            {
                                ModelState.AddModelError(memberName, validationResult.ErrorMessage);
                            }
                        }
                    }

                    ModelState.AddModelError(String.Empty, "Fel inträffade när kontakt skulle läggas till.");
                }
            }
        }

        //Metod som uppdaterar en kontakts uppgifter.
        public void ContactListView_UpdateItem(int ContactID)
        {
            try
            {
                var contact = Service.GetContact(ContactID);
                if(contact == null)
                {
                    ModelState.AddModelError(String.Empty,
                        String.Format("Kunden med kundnummer {0} hittades inte.", ContactID));
                    return;
                }

                if(TryUpdateModel(contact))
                {
                    Service.SaveContact(contact);
                    Session["Status"] = true;
                    var status = "Kontakten har uppdaterats!";
                    Response.Redirect("~/Default.aspx?status=" + status);
                }
            }
            catch(Exception)
            {
                ModelState.AddModelError(String.Empty, "Oväntat fel inträffade när kontaktuppgiften skulle uppdateras");
            }
        }

        //Metod som raderar en kontakt.
        public void ContactListView_DeleteItem(int ContactID)
        {
            try
            {
                Service.DeleteContact(ContactID);
                Session["Status"] = true;
                var status = "Kontakten har tagits bort!";
                Response.Redirect("~/Default.aspx?status=" + status);
            }
            catch(Exception)
            {
                ModelState.AddModelError(String.Empty, "Ett oväntat fel inträfade när kontaktuppgiften skulle raderas.");
            }
        }
    }
}