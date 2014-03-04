using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public void SaveContact(Contact Contact)
        {
            if (Contact.ContactId == 0)
            {
                ContactDAL.InsertContact(Contact);
            }
            else
            { 
                ContactDAL.UpdateContact(Contact);
            }
        }

        public void DeleteContact(int contactID)
        {
            ContactDAL.DeleteContact(contactID);
        }
    }
}