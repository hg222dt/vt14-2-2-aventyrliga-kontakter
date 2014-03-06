using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using contacts.Model.DAL;


namespace contacts.Model
{
    public class Service
    {
        //Fält som refererar till dataåtkomstlagret
        private ContactDAL _contactDAL;

        //Fält som skapar objekt i dataåtkomstklassen. Och koppplar referns till detta.
        private ContactDAL ContactDAL
        {
            get { return _contactDAL ?? (_contactDAL = new ContactDAL()); }
        }
        
        //Hämtar specifik kontakt från dataåtkomstlagret.
        public Contact GetContact(int ContactID)
        {
            return ContactDAL.GetContactById(ContactID);
        }

        //Hätar alla kontakter från DAL.
        public IEnumerable<Contact> GetContacts()
        {
            return ContactDAL.GetContacts();
        }

        //Hämtar kontakter sidovis från DAL
        public IEnumerable<Contact> GetContactsPageWise(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            return ContactDAL.GetContactsPageWise(maximumRows, startRowIndex, out totalRowCount);
        }

        //Sparar eller uppdaterar kontakt  via DAL
        public void SaveContact(Contact contact)
        {
            ICollection<ValidationResult> validationResults;
            if(!contact.Validate(out validationResults))
            {
                var ex = new ValidationException("Objektet klararde inte valideringen.");
                ex.Data.Add("ValidationResults", validationResults);
                throw ex;
            }

            if (contact.ContactId == 0)
            {
                ContactDAL.InsertContact(contact);
            }
            else
            { 
                ContactDAL.UpdateContact(contact);
            }
        }

        //Tar bort kontakt via DAL
        public void DeleteContact(int contactID)
        {
            ContactDAL.DeleteContact(contactID);
        }
    }
}