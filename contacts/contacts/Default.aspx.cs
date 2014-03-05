using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IEnumerable<Contact> ContactListView_GetData()
        {
            return Service.GetContacts(); 
        }

        public IEnumerable<Contact> ContactListView_GetDataPageWise(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            return Service.GetContactsPageWise(maximumRows, startRowIndex, out totalRowCount);
        }

        public void ContactListView_InsertItem(Contact Contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Service.SaveContact(Contact);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(String.Empty, "Fel inträffade när kontakt skulle läggas till.");
                }
            }
        }

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
                }
            }
            catch(Exception)
            {
                ModelState.AddModelError(String.Empty, "Oväntat fel inträffade när kontaktuppgiften skulle uppdateras");
            }
        }

        public void ContactListView_DeleteItem(int ContactID)
        {
            try
            {
                Service.DeleteContact(ContactID);
            }
            catch(Exception)
            {
                ModelState.AddModelError(String.Empty, "Ett oväntat fel inträfade när kontaktuppgiften skulle raderas.");
            }
        }
    }
}