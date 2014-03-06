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
        private ContactDAL _contactDAL;

        private ContactDAL ContactDAL
        {
            get { return _contactDAL ?? (_contactDAL = new ContactDAL()); }
        }
        
        public Contact GetContact(int ContactID)
        {
            return ContactDAL.GetContactById(ContactID);
        }

        public IEnumerable<Contact> GetContacts()
        {
            return ContactDAL.GetContacts();
        }

        public IEnumerable<Contact> GetContactsPageWise(int maximumRows, int startRowIndex, out int totalRowCount)
        {
            return ContactDAL.GetContactsPageWise(maximumRows, startRowIndex, out totalRowCount);
        }

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

        //public static void Myvalidate(object instance)
        //{
        //    var validationContext = new ValidationContext(instance);
        //    var validationResults = new List<ValidationResult>();

        //    if(!Validator.TryValidateObject(instance, validationContext, validationResults true))
        //    {
        //        var ex = new ValidationException("Objektet klarade inte valideringen.");

        //        ex.Data.Add("ValidationResults", validationResults);

        //        throw ex;
        //    }
        //}

        public void DeleteContact(int contactID)
        {
            ContactDAL.DeleteContact(contactID);
        }
    }
}