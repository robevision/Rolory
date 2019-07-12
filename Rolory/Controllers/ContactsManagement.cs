using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace Rolory.Controllers
{
    public class ContactsManagement
    {
        public List<SelectListItem> stateList = new List<SelectListItem>();
        public List<SelectListItem> countryList = new List<SelectListItem>();
        public List<SelectListItem> typeList = new List<SelectListItem>();
        public List<SelectListItem> genderList = new List<SelectListItem>();
        public List<SelectListItem> relationshipList = new List<SelectListItem>();
        public List<SelectListItem> categoryList = new List<SelectListItem>();
        public List<SelectListItem> prefixList = new List<SelectListItem>();
        public ContactsManagement()
        {
            GetStateSelection();
            GetTypeSelection();
            GetGenderSelection();
            GetRelationshipSelection();
            GetCategorySelection();
            GetPrefixSelection();
            
        }
        private void GetRelationshipSelection()
        {
            relationshipList.Add(new SelectListItem() { Text = "Distant", Value = "Distant" });
            relationshipList.Add(new SelectListItem() { Text = "Familiar", Value = "Familiar" });
            relationshipList.Add(new SelectListItem() { Text = "Friend", Value = "Friend" });
            relationshipList.Add(new SelectListItem() { Text = "Close", Value = "Close" });
            relationshipList.Add(new SelectListItem() { Text = "Business Superior", Value = "Business Superior" });
            relationshipList.Add(new SelectListItem() { Text = "Business Equal", Value = "Business Equal" });
            relationshipList.Add(new SelectListItem() { Text = "Teacher", Value = "Teacher" });
            relationshipList.Add(new SelectListItem() { Text = "Classmate", Value = "Classmate" });

        }
        private void GetCategorySelection()
        {
            categoryList.Add(new SelectListItem() { Text = "School", Value = "School" });
            categoryList.Add(new SelectListItem() { Text = "Family", Value = "Family" });
            categoryList.Add(new SelectListItem() { Text = "Social", Value = "Social" });
            categoryList.Add(new SelectListItem() { Text = "Work", Value = "Work" });
            //Be able to add custom category in the future
            categoryList.Add(new SelectListItem() { Text = "Other", Value = "Other" });
        }
        private void GetGenderSelection()
        {
            genderList.Add(new SelectListItem() { Text = "Male", Value = "Male" });
            genderList.Add(new SelectListItem() { Text = "Female", Value = "Female" });
        }
        private void GetTypeSelection()
        {
            typeList.Add(new SelectListItem() { Text = "Home", Value = "Home" });
            typeList.Add(new SelectListItem() { Text = "Work", Value = "Work" });
            typeList.Add(new SelectListItem() { Text = "Other", Value = "Other" });
        }
        private void GetStateSelection()
        {
            stateList.Add(new SelectListItem() { Text = "Alabama", Value = "AL" });
            stateList.Add(new SelectListItem() { Text = "Alaska", Value = "AK" });
            stateList.Add(new SelectListItem() { Text = "Arizona", Value = "AZ" });
            stateList.Add(new SelectListItem() { Text = "Arkansas", Value = "AR" });
            stateList.Add(new SelectListItem() { Text = "California", Value = "CA" });
            stateList.Add(new SelectListItem() { Text = "Colorado", Value = "CO" });
            stateList.Add(new SelectListItem() { Text = "Connecticut", Value = "CT" });
            stateList.Add(new SelectListItem() { Text = "Delaware", Value = "DE" });
            stateList.Add(new SelectListItem() { Text = "Florida", Value = "FL" });
            stateList.Add(new SelectListItem() { Text = "Georgia", Value = "GA" });
            stateList.Add(new SelectListItem() { Text = "Hawaii", Value = "HI" });
            stateList.Add(new SelectListItem() { Text = "Idaho", Value = "ID" });
            stateList.Add(new SelectListItem() { Text = "Illinois", Value = "IL" });
            stateList.Add(new SelectListItem() { Text = "Indiana", Value = "IN" });
            stateList.Add(new SelectListItem() { Text = "Iowa", Value = "IA" });
            stateList.Add(new SelectListItem() { Text = "Kansas", Value = "KS" });
            stateList.Add(new SelectListItem() { Text = "Kentucky", Value = "KY" });
            stateList.Add(new SelectListItem() { Text = "Louisiana", Value = "LA" });
            stateList.Add(new SelectListItem() { Text = "Maine", Value = "ME" });
            stateList.Add(new SelectListItem() { Text = "Maryland", Value = "MD" });
            stateList.Add(new SelectListItem() { Text = "Massachusetts", Value = "MA" });
            stateList.Add(new SelectListItem() { Text = "Michigan", Value = "MI" });
            stateList.Add(new SelectListItem() { Text = "Minnesota", Value = "MN" });
            stateList.Add(new SelectListItem() { Text = "Connecticut", Value = "MS" });
            stateList.Add(new SelectListItem() { Text = "Missouri", Value = "MO" });
            stateList.Add(new SelectListItem() { Text = "Montana", Value = "MT" });
            stateList.Add(new SelectListItem() { Text = "Nebraska", Value = "NE" });
            stateList.Add(new SelectListItem() { Text = "Nevada", Value = "NV" });
            stateList.Add(new SelectListItem() { Text = "New Hampshire", Value = "NH" });
            stateList.Add(new SelectListItem() { Text = "New Jersey", Value = "NJ" });
            stateList.Add(new SelectListItem() { Text = "New York", Value = "NY" });
            stateList.Add(new SelectListItem() { Text = "New Mexico", Value = "NM" });
            stateList.Add(new SelectListItem() { Text = "North Carolina", Value = "NC" });
            stateList.Add(new SelectListItem() { Text = "North Dakota", Value = "ND" });
            stateList.Add(new SelectListItem() { Text = "Ohio", Value = "OH" });
            stateList.Add(new SelectListItem() { Text = "Oklahoma", Value = "OK" });
            stateList.Add(new SelectListItem() { Text = "Oregon", Value = "OR" });
            stateList.Add(new SelectListItem() { Text = "Pennsylvania", Value = "PA" });
            stateList.Add(new SelectListItem() { Text = "Rhode Island", Value = "RI" });
            stateList.Add(new SelectListItem() { Text = "South Carolina", Value = "SC" });
            stateList.Add(new SelectListItem() { Text = "South Dakota", Value = "SD" });
            stateList.Add(new SelectListItem() { Text = "Tennessee", Value = "TN" });
            stateList.Add(new SelectListItem() { Text = "Texas", Value = "TX" });
            stateList.Add(new SelectListItem() { Text = "Utah", Value = "UT" });
            stateList.Add(new SelectListItem() { Text = "Vermont", Value = "VT" });
            stateList.Add(new SelectListItem() { Text = "Virginia", Value = "VA" });
            stateList.Add(new SelectListItem() { Text = "Washington", Value = "WA" });
            stateList.Add(new SelectListItem() { Text = "West Virginia", Value = "WV" });
            stateList.Add(new SelectListItem() { Text = "Wisconsin", Value = "WI" });
            stateList.Add(new SelectListItem() { Text = "Wyoming", Value = "WY" });
        }
        private void GetCountrySelection()
        {
            countryList.Add(new SelectListItem() { Text = "Afghanistan", Value = "Afghanistan" });
            countryList.Add(new SelectListItem() { Text = "Albania", Value = "Albania" });
            countryList.Add(new SelectListItem() { Text = "Algeria", Value = "Algeria" });
            countryList.Add(new SelectListItem() { Text = "Andorra", Value = "Andorra" });
            countryList.Add(new SelectListItem() { Text = "Angola", Value = "Angola" });
            countryList.Add(new SelectListItem() { Text = "Antigua", Value = "Afghanistan" });
            countryList.Add(new SelectListItem() { Text = "Argentina", Value = "Argentina" });
            countryList.Add(new SelectListItem() { Text = "Armenia", Value = "Armenia" });
            countryList.Add(new SelectListItem() { Text = "Australia", Value = "Australia" });
            countryList.Add(new SelectListItem() { Text = "Austria", Value = "Austria" });
            countryList.Add(new SelectListItem() { Text = "Azerbaijan", Value = "Azerbaijan" });
            countryList.Add(new SelectListItem() { Text = "Bahamas", Value = "The Bahamas" });
            countryList.Add(new SelectListItem() { Text = "Bahrain", Value = "Bahrain" });
            countryList.Add(new SelectListItem() { Text = "Bangladesh", Value = "Bangladesh" });
            countryList.Add(new SelectListItem() { Text = "Barbuda", Value = "Barbuda" });
            countryList.Add(new SelectListItem() { Text = "Barbados", Value = "Barbados" });
            countryList.Add(new SelectListItem() { Text = "Belarus", Value = "Belarus" });
            countryList.Add(new SelectListItem() { Text = "Belgium", Value = "Belgium" });
            countryList.Add(new SelectListItem() { Text = "Belize", Value = "Belize" });
            countryList.Add(new SelectListItem() { Text = "Benin", Value = "Benin" });
            countryList.Add(new SelectListItem() { Text = "Bhutan", Value = "Bhutan" });
            countryList.Add(new SelectListItem() { Text = "Bolivia", Value = "Bolivia" });
            countryList.Add(new SelectListItem() { Text = "Bosnia", Value = "Bosnia" });
            countryList.Add(new SelectListItem() { Text = "Botswana", Value = "Botswana" });
            countryList.Add(new SelectListItem() { Text = "Brazil", Value = "Brazil" });
            countryList.Add(new SelectListItem() { Text = "Brunei", Value = "Brunei" });
            countryList.Add(new SelectListItem() { Text = "Bulgaria", Value = "Bulgaria" });
            countryList.Add(new SelectListItem() { Text = "Burkina Faso", Value = "Burkina Faso" });
            countryList.Add(new SelectListItem() { Text = "Burundi", Value = "Burundi" });
            countryList.Add(new SelectListItem() { Text = "Cabo Verde", Value = "Cabo Verde" });
            countryList.Add(new SelectListItem() { Text = "Cambodia", Value = "Cambodia" });
            countryList.Add(new SelectListItem() { Text = "Cameroon", Value = "Cameroon" });

        }
        private void GetPrefixSelection()
        {
            prefixList.Add(new SelectListItem() { Text = "Dr.", Value = "Dr." });
            prefixList.Add(new SelectListItem() { Text = "Mr.", Value = "Mr." });
            prefixList.Add(new SelectListItem() { Text = "Mrs.", Value = "Mrs." });
            prefixList.Add(new SelectListItem() { Text = "Ms.", Value = "Ms." });
            prefixList.Add(new SelectListItem() { Text = "Miss", Value = "Miss" });
            prefixList.Add(new SelectListItem() { Text = "Master", Value = "Master" });
        }

    }
}