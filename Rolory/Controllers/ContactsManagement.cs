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
        public List<double[]> latLongList = new List<double[]>();
        public ContactsManagement()
        {
            GetStateSelection();
            GetTypeSelection();
            GetGenderSelection();
            GetRelationshipSelection();
            GetCategorySelection();
            GetPrefixSelection();
            GetLatLongFromRegion();
            
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
            stateList.Add(new SelectListItem() { Text = "Mississippi", Value = "MS" });
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
        private void GetLatLongFromRegion()
        {
            latLongList.Add(new double[2] {32.3182, -86.9023 });//alabama1
            latLongList.Add(new double[2] { 64.2008, -149.4937 });//alaska2
            latLongList.Add(new double[2] { 34.0489, -111.0937 });//3
            latLongList.Add(new double[2] { 35.2010, -91.8318 });//4
            latLongList.Add(new double[2] { 36.7783, -119.4179 });//5
            latLongList.Add(new double[2] { 39.5501, -105.7821 });//6
            latLongList.Add(new double[2] { 41.6032, -73.0877 });//7
            latLongList.Add(new double[2] { 38.9108, -75.5277 });//8
            latLongList.Add(new double[2] { 27.6648, -81.5158 });//9
            latLongList.Add(new double[2] { 32.1656, -82.9001 });//10
            latLongList.Add(new double[2] { 19.8968, -155.5828 });//11
            latLongList.Add(new double[2] { 44.0682, -114.7420 });//12
            latLongList.Add(new double[2] { 40.6331, -89.3985 });//13
            latLongList.Add(new double[2] { 40.2672, -86.1349 });//14
            latLongList.Add(new double[2] { 41.8780, -93.0977 });//15
            latLongList.Add(new double[2] { 39.0119, -98.4842 });//16
            latLongList.Add(new double[2] { 37.8393, -84.2700 });//17
            latLongList.Add(new double[2] { 30.9843, -91.9623 });//18
            latLongList.Add(new double[2] { 45.2538, -69.4455 });//19
            latLongList.Add(new double[2] { 39.0458, -76.6413 });//20
            latLongList.Add(new double[2] { 42.4072, -71.3824 });//21
            latLongList.Add(new double[2] { 44.3148, -85.6024 });//22
            latLongList.Add(new double[2] { 46.7296, -94.6859 });//23
            latLongList.Add(new double[2] { 32.3547, -89.3985 });//24
            latLongList.Add(new double[2] { 37.9643, -91.8318 });//25
            latLongList.Add(new double[2] { 46.8797, -110.3626 });//26
            latLongList.Add(new double[2] { 41.4925, -99.9018 });//27
            latLongList.Add(new double[2] { 38.8026, -116.4194 });
            latLongList.Add(new double[2] { 43.1939, -71.5724 });
            latLongList.Add(new double[2] { 40.0583, -74.4057 });
            latLongList.Add(new double[2] { 40.7128, -74.0060 });
            latLongList.Add(new double[2] { 34.5199, -105.8701 });
            latLongList.Add(new double[2] { 35.7596, -79.0193 });
            latLongList.Add(new double[2] { 47.5515, -101.0020 });
            latLongList.Add(new double[2] { 40.4173, -82.9071});
            latLongList.Add(new double[2] { 35.0078, -97.0929 });
            latLongList.Add(new double[2] { 43.8041, -120.5542 });
            latLongList.Add(new double[2] { 41.2033, -77.1945 });
            latLongList.Add(new double[2] { 41.5801, -71.4774 });
            latLongList.Add(new double[2] { 33.8361, -81.1637 });
            latLongList.Add(new double[2] { 43.9695, -99.9018});
            latLongList.Add(new double[2] { 35.5175, -86.5804 });
            latLongList.Add(new double[2] { 31.9686, -99.9018 });
            latLongList.Add(new double[2] { 39.3210, -111.0937 });
            latLongList.Add(new double[2] { 44.5588, -72.5778 });
            latLongList.Add(new double[2] { 37.4316, -78.6569 });
            latLongList.Add(new double[2] { 47.7511, -120.7401});
            latLongList.Add(new double[2] { 38.5976, -80.4549 });
            latLongList.Add(new double[2] { 43.7844, -88.7879 });
            latLongList.Add(new double[2] { 43.0760, -107.2903 });
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
        public string ReturnOnlyIntegers(string number)
        {
            string newNumber = "";
            if(number != null)
            {
                var openParentheses = "(";
                var closedParentheses = ")";
                var dash = "-";
                var space = " ";
                //attempting to build alternative option to where all values are in an array. Need to research how to store an array of char 
                var characters = " )(-".ToCharArray();
                for (int i = 0; i < number.Length; i++)
                {

                    if (number[i] != openParentheses[0] && number[i] != closedParentheses[0] && number[i] != dash[0] && number[i] != space[0])
                    {
                        newNumber = newNumber + number[i];

                    }
                }

            }
           
            return newNumber;
        } 
        public string PopulatePhoneNumber(string number)
        {
            if (String.IsNullOrEmpty(number) != true)
            {
                List<string> phoneNumberResult = new List<string>();
                List<char> areaCode = new List<char>();
                List<char> body = new List<char>();
                //you may need to move this to the method that it is being called from
                //var phoneNumber = contact.PhoneNumber;
                var phoneNumber = number;
                for (int i = 0; i < phoneNumber.Count(); i++)
                {
                    if (Char.IsNumber(phoneNumber[i]) == true)
                    {
                        if (i == 0)
                        {
                            areaCode.Add(Convert.ToChar("("));
                            areaCode.Add(phoneNumber[i]);
                        }
                        else if (i == 1)
                        {
                            areaCode.Add(phoneNumber[i]);
                        }
                        else if (i == 2)
                        {
                            areaCode.Add(phoneNumber[i]);
                            areaCode.Add(Convert.ToChar(")"));
                            areaCode.Add(Convert.ToChar(" "));
                        }
                        else if (i > 2)
                        {
                            if (body.LongCount() == 3)
                            {
                                body.Add(Convert.ToChar("-"));
                            }
                            if (body.LongCount() < 8)
                            {
                                body.Add(phoneNumber[i]);
                            }

                        }

                    }
                }
                foreach (char index in areaCode)
                {
                    phoneNumberResult.Add(Convert.ToString(index));
                }
                foreach (char index in body)
                {
                    phoneNumberResult.Add(Convert.ToString(index));
                }
                return String.Join("", phoneNumberResult.ToArray());
            }
            return number;
        }
    }
}