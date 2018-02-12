//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using System;
using System.Collections.Generic;

namespace eCMS.DataLogic.Seed
{
    public static partial class SeedData
    {
        public static List<Country> Countries
        {
            get
            {
                List<Country> items = new List<Country>();

                items.Add(new Country { ID = 1, Name = "United States", Code = "USA" });
                items.Add(new Country { ID = 2, Name = "United Kingdom", Code = "UK" });
                items.Add(new Country { ID = 3, Name = "United Arab Emirates", Code = "UAE" });
                items.Add(new Country { ID = 4, Name = "Swaziland", Code = "SWZ" });
                items.Add(new Country { ID = 5, Name = "India", Code = "IND" });
                items.Add(new Country { ID = 6, Name = "France", Code = "FRA" });
                items.Add(new Country { ID = 7, Name = "Canada", Code = "CAN" });
                items.Add(new Country { ID = 8, Name = "Australia", Code = "AUS" });
                items.Add(new Country { ID = 9, Name = "China", Code = "CHN" });
                items.Add(new Country { ID = 10, Name = "Germany", Code = "GER" });
                items.Add(new Country { ID = 11, Name = "Austria", Code = "AUT" });
                items.Add(new Country { ID = 12, Name = "Azerbaijan", Code = "AZE" });
                items.Add(new Country { ID = 13, Name = "Bahamas", Code = "BHS" });
                items.Add(new Country { ID = 14, Name = "Bahrain", Code = "BHR" });
                items.Add(new Country { ID = 15, Name = "Bangladesh", Code = "BGD" });
                items.Add(new Country { ID = 16, Name = "Barbados", Code = "BRB" });
                items.Add(new Country { ID = 17, Name = "Belarus", Code = "BLR" });
                items.Add(new Country { ID = 18, Name = "Belgium", Code = "BEL" });
                items.Add(new Country { ID = 19, Name = "Belize", Code = "BLZ" });
                items.Add(new Country { ID = 20, Name = "Benin", Code = "BEN" });
                items.Add(new Country { ID = 21, Name = "Bermuda", Code = "BMU" });
                items.Add(new Country { ID = 22, Name = "Bhutan", Code = "BTN" });
                items.Add(new Country { ID = 23, Name = "Bolivia", Code = "BOL" });
                items.Add(new Country { ID = 24, Name = "Bosnia and Herzegovina", Code = "BIH" });
                items.Add(new Country { ID = 25, Name = "Botswana", Code = "BWA" });
                items.Add(new Country { ID = 26, Name = "Bouvet Island", Code = "BVT" });
                items.Add(new Country { ID = 27, Name = "Brazil", Code = "BRA" });
                items.Add(new Country { ID = 28, Name = "British Virgin Islands", Code = "VGB" });
                items.Add(new Country { ID = 29, Name = "Brunei Darussalam", Code = "BRN" });
                items.Add(new Country { ID = 30, Name = "Bulgaria", Code = "BGR" });
                items.Add(new Country { ID = 31, Name = "Burkina Faso", Code = "BFA" });
                items.Add(new Country { ID = 32, Name = "Burundi", Code = "BDI" });
                items.Add(new Country { ID = 33, Name = "Cambodia", Code = "KHM" });
                items.Add(new Country { ID = 34, Name = "Cameroon", Code = "CMR" });
                items.Add(new Country { ID = 35, Name = "Cape Verde", Code = "CPV" });
                items.Add(new Country { ID = 36, Name = "Cayman Islands", Code = "CYM" });
                items.Add(new Country { ID = 37, Name = "Central African Republic", Code = "CAF" });
                items.Add(new Country { ID = 38, Name = "Chad", Code = "TCD" });
                items.Add(new Country { ID = 39, Name = "Chile", Code = "CHL" });
                items.Add(new Country { ID = 40, Name = "Colombia", Code = "COL" });
                items.Add(new Country { ID = 41, Name = "Comoros", Code = "COM" });
                items.Add(new Country { ID = 42, Name = "Congo", Code = "COG" });
                items.Add(new Country { ID = 43, Name = "Cook Islands", Code = "COK" });
                items.Add(new Country { ID = 44, Name = "Costa Rica", Code = "CRI" });
                items.Add(new Country { ID = 45, Name = "Cote d'Ivoire", Code = "CIV" });
                items.Add(new Country { ID = 46, Name = "Croatia", Code = "HRV" });
                items.Add(new Country { ID = 47, Name = "Cuba", Code = "CUB" });
                items.Add(new Country { ID = 48, Name = "Cyprus", Code = "CYP" });
                items.Add(new Country { ID = 49, Name = "Czech Republic", Code = "CZE" });
                items.Add(new Country { ID = 50, Name = "Denmark", Code = "DNK" });
                items.Add(new Country { ID = 51, Name = "Djibouti", Code = "DJI" });
                items.Add(new Country { ID = 52, Name = "Dominica", Code = "DMA" });
                items.Add(new Country { ID = 53, Name = "Dominican Republic", Code = "DOM" });
                items.Add(new Country { ID = 54, Name = "East Timor", Code = "TLS" });
                items.Add(new Country { ID = 55, Name = "Ecuador", Code = "ECU" });
                items.Add(new Country { ID = 56, Name = "Egypt", Code = "EGY" });
                items.Add(new Country { ID = 57, Name = "El Salvador", Code = "SLV" });
                items.Add(new Country { ID = 58, Name = "Equatorial Guinea", Code = "GNQ" });
                items.Add(new Country { ID = 59, Name = "Eritrea", Code = "ERI" });
                items.Add(new Country { ID = 60, Name = "Estonia", Code = "EST" });
                items.Add(new Country { ID = 61, Name = "Ethiopia", Code = "ETH" });
                items.Add(new Country { ID = 62, Name = "Europe	Kosovo", Code = "EKV" });
                items.Add(new Country { ID = 63, Name = "Faeroe Islands", Code = "FRO" });
                items.Add(new Country { ID = 64, Name = "Fiji", Code = "FJI" });
                items.Add(new Country { ID = 65, Name = "Finland", Code = "FIN" });
                items.Add(new Country { ID = 66, Name = "France Metropolitan", Code = "FXX" });
                items.Add(new Country { ID = 67, Name = "French Guiana", Code = "GUF" });
                items.Add(new Country { ID = 68, Name = "French Polynesia", Code = "PYF" });
                items.Add(new Country { ID = 69, Name = "Gabon", Code = "GAB" });
                items.Add(new Country { ID = 70, Name = "Gambia", Code = "GMB" });
                items.Add(new Country { ID = 71, Name = "Haiti", Code = "HTI" });
                items.Add(new Country { ID = 72, Name = "Honduras", Code = "HND" });
                items.Add(new Country { ID = 73, Name = "Hong Kong", Code = "HKG" });
                items.Add(new Country { ID = 74, Name = "Hungary", Code = "HUN" });
                items.Add(new Country { ID = 75, Name = "Iceland", Code = "ISL" });
                items.Add(new Country { ID = 76, Name = "Indonesia", Code = "IDN" });
                items.Add(new Country { ID = 77, Name = "Iran", Code = "IRN" });
                items.Add(new Country { ID = 78, Name = "Iraq", Code = "IRQ" });
                items.Add(new Country { ID = 79, Name = "Ireland", Code = "IRL" });
                items.Add(new Country { ID = 80, Name = "Israel", Code = "ISR" });
                items.Add(new Country { ID = 81, Name = "Italy", Code = "ITA" });
                items.Add(new Country { ID = 82, Name = "Jamaica", Code = "JAM" });
                items.Add(new Country { ID = 83, Name = "Japan", Code = "JPN" });
                items.Add(new Country { ID = 84, Name = "Jordan", Code = "JOR" });
                items.Add(new Country { ID = 85, Name = "Kazakhstan", Code = "KAZ" });
                items.Add(new Country { ID = 86, Name = "Kenya", Code = "KEN" });
                items.Add(new Country { ID = 87, Name = "Kiribati", Code = "KIR" });
                items.Add(new Country { ID = 88, Name = "Kuwait", Code = "KWT" });
                items.Add(new Country { ID = 89, Name = "Kyrgyzstan", Code = "KGZ" });
                items.Add(new Country { ID = 90, Name = "Laos", Code = "LAO" });
                items.Add(new Country { ID = 91, Name = "Latvia", Code = "" });
                items.Add(new Country { ID = 92, Name = "Lebanon", Code = "LBN" });
                items.Add(new Country { ID = 93, Name = "Lesotho", Code = "LSO" });
                items.Add(new Country { ID = 94, Name = "Liberia", Code = "LBR" });
                items.Add(new Country { ID = 95, Name = "Libya", Code = "LBY" });
                items.Add(new Country { ID = 96, Name = "Liechtenstein", Code = "LIE" });
                items.Add(new Country { ID = 97, Name = "Lithuania", Code = "LTU" });
                items.Add(new Country { ID = 98, Name = "Luxembourg", Code = "LUX" });
                items.Add(new Country { ID = 99, Name = "Macau", Code = "MAC" });
                items.Add(new Country { ID = 100, Name = "Macedonia", Code = "MKD" });
                items.Add(new Country { ID = 101, Name = "Madagascar", Code = "MDG" });
                items.Add(new Country { ID = 102, Name = "Malawi", Code = "MWI" });
                items.Add(new Country { ID = 103, Name = "Malaysia", Code = "MYS" });
                items.Add(new Country { ID = 104, Name = "Maldives", Code = "MDV" });
                items.Add(new Country { ID = 105, Name = "Mali", Code = "MLI" });
                items.Add(new Country { ID = 106, Name = "Malta", Code = "MLT" });
                items.Add(new Country { ID = 107, Name = "Marshall Islands", Code = "MHL" });
                items.Add(new Country { ID = 108, Name = "Martinique", Code = "MTQ" });
                items.Add(new Country { ID = 109, Name = "Mauritania", Code = "MRT" });
                items.Add(new Country { ID = 110, Name = "Mauritius", Code = "MUS" });
                items.Add(new Country { ID = 111, Name = "Mayotte", Code = "MYT" });
                items.Add(new Country { ID = 112, Name = "Mexico", Code = "MEX" });
                items.Add(new Country { ID = 113, Name = "Micronesia", Code = "FSM" });
                items.Add(new Country { ID = 114, Name = "Moldova", Code = "MDA" });
                items.Add(new Country { ID = 115, Name = "Monaco", Code = "MCO" });
                items.Add(new Country { ID = 116, Name = "Mongolia", Code = "MNG" });
                items.Add(new Country { ID = 117, Name = "Montenegro", Code = "MNE" });
                items.Add(new Country { ID = 118, Name = "Montserrat", Code = "MSR" });
                items.Add(new Country { ID = 119, Name = "Morocco", Code = "MAR" });
                items.Add(new Country { ID = 120, Name = "Mozambique", Code = "MOZ" });
                items.Add(new Country { ID = 121, Name = "Myanmar", Code = "MMR" });
                items.Add(new Country { ID = 122, Name = "Namibia", Code = "NAM" });
                items.Add(new Country { ID = 123, Name = "Nauru", Code = "NRU" });
                items.Add(new Country { ID = 124, Name = "Nepal", Code = "NPL" });
                items.Add(new Country { ID = 125, Name = "Netherlands", Code = "NLD" });
                items.Add(new Country { ID = 126, Name = "Netherlands Antilles", Code = "ANT" });
                items.Add(new Country { ID = 127, Name = "New Caledonia", Code = "NCL" });
                items.Add(new Country { ID = 128, Name = "New Zealand", Code = "NZL" });
                items.Add(new Country { ID = 129, Name = "Nicaragua", Code = "NIC" });
                items.Add(new Country { ID = 130, Name = "Niger", Code = "NER" });
                items.Add(new Country { ID = 131, Name = "Nigeria", Code = "NGA" });
                items.Add(new Country { ID = 132, Name = "Niue", Code = "NIU" });
                items.Add(new Country { ID = 133, Name = "Norfolk Island", Code = "NFK" });
                items.Add(new Country { ID = 134, Name = "North Korea", Code = "PRK" });
                items.Add(new Country { ID = 135, Name = "Northern Mariana Islands", Code = "MNP" });
                items.Add(new Country { ID = 136, Name = "Portugal", Code = "PRT" });
                items.Add(new Country { ID = 137, Name = "Puerto Rico", Code = "PRI" });
                items.Add(new Country { ID = 138, Name = "Qatar", Code = "QAT" });
                items.Add(new Country { ID = 139, Name = "Republic of Georgia", Code = "GEO" });
                items.Add(new Country { ID = 140, Name = "Reunion", Code = "REU" });
                items.Add(new Country { ID = 141, Name = "Romania", Code = "ROU" });
                items.Add(new Country { ID = 142, Name = "Russia", Code = "RUS" });
                items.Add(new Country { ID = 143, Name = "Rwanda", Code = "RWA" });
                items.Add(new Country { ID = 144, Name = "Samoa", Code = "WSM" });
                items.Add(new Country { ID = 145, Name = "San Marino", Code = "SMR" });
                items.Add(new Country { ID = 146, Name = "Saudi Arabia", Code = "KSA" });
                items.Add(new Country { ID = 147, Name = "Senegal", Code = "SEN" });
                items.Add(new Country { ID = 148, Name = "Serbia", Code = "SRB" });
                items.Add(new Country { ID = 149, Name = "Seychelles", Code = "SYC" });
                items.Add(new Country { ID = 150, Name = "Sierra Leone", Code = "SLE" });
                items.Add(new Country { ID = 151, Name = "Singapore", Code = "SGP" });
                items.Add(new Country { ID = 152, Name = "Slovakia", Code = "SVK" });
                items.Add(new Country { ID = 153, Name = "Slovenia", Code = "SVN" });
                items.Add(new Country { ID = 154, Name = "Solomon Islands", Code = "SLB" });
                items.Add(new Country { ID = 155, Name = "Somalia", Code = "SOM" });
                items.Add(new Country { ID = 156, Name = "South Africa", Code = "ZAF" });
                items.Add(new Country { ID = 157, Name = "South Georgia", Code = "SGS" });
                items.Add(new Country { ID = 158, Name = "South Korea", Code = "KOR" });
                items.Add(new Country { ID = 159, Name = "Spain", Code = "ESP" });
                items.Add(new Country { ID = 160, Name = "Sri Lanka", Code = "LKA" });
                items.Add(new Country { ID = 161, Name = "St. Helena", Code = "SHN" });
                items.Add(new Country { ID = 162, Name = "St. Kitts and Nevis", Code = "KNA" });
                items.Add(new Country { ID = 163, Name = "St. Lucia", Code = "LCA" });
                items.Add(new Country { ID = 164, Name = "Sudan", Code = "SDN" });
                items.Add(new Country { ID = 165, Name = "Suriname", Code = "SUR" });
                items.Add(new Country { ID = 166, Name = "Sweden", Code = "SWE" });
                items.Add(new Country { ID = 167, Name = "Switzerland", Code = "CHE" });
                items.Add(new Country { ID = 168, Name = "Syria", Code = "SYR" });
                items.Add(new Country { ID = 169, Name = "Taiwan", Code = "TWN" });
                items.Add(new Country { ID = 170, Name = "Tajikistan", Code = "TJK" });
                items.Add(new Country { ID = 171, Name = "Tanzania", Code = "TZA" });
                items.Add(new Country { ID = 172, Name = "Thailand", Code = "THA" });
                items.Add(new Country { ID = 173, Name = "Togo", Code = "TGO" });
                items.Add(new Country { ID = 174, Name = "Tokelau", Code = "TKL" });
                items.Add(new Country { ID = 175, Name = "Tonga", Code = "TON" });
                items.Add(new Country { ID = 176, Name = "Trinidad and Tobago", Code = "TTO" });
                items.Add(new Country { ID = 177, Name = "Tunisia", Code = "TUN" });
                items.Add(new Country { ID = 178, Name = "Turkey", Code = "TUR" });
                items.Add(new Country { ID = 179, Name = "Turkmenistan", Code = "TKM" });
                items.Add(new Country { ID = 180, Name = "Tuvalu", Code = "TUV" });
                items.Add(new Country { ID = 181, Name = "U.S. Virgin Islands", Code = "VIR" });
                items.Add(new Country { ID = 182, Name = "Uganda", Code = "UGA" });
                items.Add(new Country { ID = 183, Name = "Ukraine", Code = "UKR" });
                items.Add(new Country { ID = 184, Name = "Uruguay", Code = "URY" });
                items.Add(new Country { ID = 185, Name = "Uzbekistan", Code = "UZB" });
                items.Add(new Country { ID = 186, Name = "Vanuatu", Code = "VUT" });
                items.Add(new Country { ID = 187, Name = "Vatican City", Code = "VAT" });
                items.Add(new Country { ID = 188, Name = "Venezuela", Code = "VEN" });
                items.Add(new Country { ID = 189, Name = "Vietnam", Code = "VNM" });
                items.Add(new Country { ID = 190, Name = "Western Sahara", Code = "ESH" });
                items.Add(new Country { ID = 191, Name = "Yemen", Code = "YEM" });
                items.Add(new Country { ID = 192, Name = "Yugoslavia", Code = "YUG" });
                items.Add(new Country { ID = 193, Name = "Zambia", Code = "ZMB" });
                items.Add(new Country { ID = 194, Name = "Zimbabwe", Code = "ZWE" });
                items.Add(new Country { ID = 195, Name = "Philippines", Code = "PH" });

                return items;
            }
        }

        public static List<State> States
        {
            get
            {
                List<State> items = new List<State>();

                //items.Add(new State { ID = 1, Name = "Alaska", Code = "AK", CountryID = 1 });
                //items.Add(new State { ID = 2, Name = "Alabama", Code = "AL", CountryID = 1 });
                //items.Add(new State { ID = 3, Name = "Arkansas", Code = "AR", CountryID = 1 });
                //items.Add(new State { ID = 4, Name = "Arizona", Code = "AZ", CountryID = 1 });
                //items.Add(new State { ID = 5, Name = "California", Code = "CA", CountryID = 1 });
                //items.Add(new State { ID = 6, Name = "Colorado", Code = "CO", CountryID = 1 });
                //items.Add(new State { ID = 7, Name = "Connecticut", Code = "CT", CountryID = 1 });
                //items.Add(new State { ID = 8, Name = "District of Columbia", Code = "DC", CountryID = 1 });
                //items.Add(new State { ID = 9, Name = "Delaware", Code = "DE", CountryID = 1 });
                //items.Add(new State { ID = 10, Name = "Florida", Code = "FL", CountryID = 1 });
                //items.Add(new State { ID = 11, Name = "Georgia", Code = "GA", CountryID = 1 });
                //items.Add(new State { ID = 12, Name = "Hawaii", Code = "HI", CountryID = 1 });
                //items.Add(new State { ID = 13, Name = "Iowa", Code = "IA", CountryID = 1 });
                //items.Add(new State { ID = 14, Name = "Idaho", Code = "ID", CountryID = 1 });
                //items.Add(new State { ID = 15, Name = "Illinois", Code = "IL", CountryID = 1 });
                //items.Add(new State { ID = 16, Name = "Indiana", Code = "IN", CountryID = 1 });
                //items.Add(new State { ID = 17, Name = "Kansas", Code = "KS", CountryID = 1 });
                //items.Add(new State { ID = 18, Name = "Kentucky", Code = "KY", CountryID = 1 });
                //items.Add(new State { ID = 19, Name = "Louisiana", Code = "LA", CountryID = 1 });
                //items.Add(new State { ID = 20, Name = "Massachusetts", Code = "MA", CountryID = 1 });
                //items.Add(new State { ID = 21, Name = "Maryland", Code = "MD", CountryID = 1 });
                //items.Add(new State { ID = 22, Name = "Maine", Code = "ME", CountryID = 1 });
                //items.Add(new State { ID = 23, Name = "Michigan", Code = "MI", CountryID = 1 });
                //items.Add(new State { ID = 24, Name = "Minnesota", Code = "MN", CountryID = 1 });
                //items.Add(new State { ID = 25, Name = "Missouri", Code = "MO", CountryID = 1 });
                //items.Add(new State { ID = 26, Name = "Mississippi", Code = "MS", CountryID = 1 });
                //items.Add(new State { ID = 27, Name = "Montana", Code = "MT", CountryID = 1 });
                //items.Add(new State { ID = 28, Name = "North Carolina", Code = "NC", CountryID = 1 });
                //items.Add(new State { ID = 29, Name = "North Dakota", Code = "ND", CountryID = 1 });
                //items.Add(new State { ID = 30, Name = "Nebraska", Code = "NE", CountryID = 1 });
                //items.Add(new State { ID = 31, Name = "New Hampshire", Code = "NH", CountryID = 1 });
                //items.Add(new State { ID = 32, Name = "New Jersey", Code = "NJ", CountryID = 1 });
                //items.Add(new State { ID = 33, Name = "New Mexico", Code = "NM", CountryID = 1 });
                //items.Add(new State { ID = 34, Name = "Nevada", Code = "NV", CountryID = 1 });
                //items.Add(new State { ID = 35, Name = "New York", Code = "NY", CountryID = 1 });
                //items.Add(new State { ID = 36, Name = "Ohio", Code = "OH", CountryID = 1 });
                //items.Add(new State { ID = 37, Name = "Oklahoma", Code = "OK", CountryID = 1 });
                //items.Add(new State { ID = 38, Name = "Oregon", Code = "OR", CountryID = 1 });
                //items.Add(new State { ID = 39, Name = "Pennsylvania", Code = "PA", CountryID = 1 });
                //items.Add(new State { ID = 40, Name = "Rhode Island", Code = "RI", CountryID = 1 });
                //items.Add(new State { ID = 41, Name = "South Carolina", Code = "SC", CountryID = 1 });
                //items.Add(new State { ID = 42, Name = "South Dakota", Code = "SD", CountryID = 1 });
                //items.Add(new State { ID = 43, Name = "Tennessee", Code = "TN", CountryID = 1 });
                //items.Add(new State { ID = 44, Name = "Texas", Code = "TX", CountryID = 1 });
                //items.Add(new State { ID = 45, Name = "Utah", Code = "UT", CountryID = 1 });
                //items.Add(new State { ID = 46, Name = "Vermont", Code = "VT", CountryID = 1 });
                //items.Add(new State { ID = 47, Name = "Virginia", Code = "VA", CountryID = 1 });
                //items.Add(new State { ID = 48, Name = "Washington", Code = "WA", CountryID = 1 });
                //items.Add(new State { ID = 49, Name = "Wisconsin", Code = "WI", CountryID = 1 });
                //items.Add(new State { ID = 50, Name = "West Virginia", Code = "WV", CountryID = 1 });
                //items.Add(new State { ID = 51, Name = "Wyoming", Code = "WY", CountryID = 1 });

                //items.Add(new State { ID = 52, Name = "Abu Dhabi", Code = "", CountryID = 3 });
                //items.Add(new State { ID = 53, Name = "Dubai", Code = "", CountryID = 3 });
                //items.Add(new State { ID = 54, Name = "Ajman", Code = "", CountryID = 3 });
                //items.Add(new State { ID = 55, Name = "Sharjah", Code = "", CountryID = 3 });
                //items.Add(new State { ID = 56, Name = "Um Al Quwain", Code = "", CountryID = 3 });
                //items.Add(new State { ID = 57, Name = "Fujairah", Code = "", CountryID = 3 });
                //items.Add(new State { ID = 58, Name = "Ras al Khaimah", Code = "", CountryID = 3 });

                //items.Add(new State { ID = 59, Name = "Andra pradesh", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 60, Name = "Arunachal Pradesh", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 61, Name = "Assam", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 62, Name = "Bihar", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 63, Name = "Chhattisgarh", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 64, Name = "Goa", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 65, Name = "Gujarat", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 66, Name = "Haryana", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 67, Name = "Himachal Pradesh", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 68, Name = "Jammu and Kashmir", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 69, Name = "Jharkhand", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 70, Name = "Karnataka", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 71, Name = "Kerala", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 72, Name = "Madhya Pradesh", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 73, Name = "Maharashtra", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 74, Name = "Manipur", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 75, Name = "Meghalaya", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 76, Name = "Mizoram", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 77, Name = "Nagaland", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 78, Name = "Orissa", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 79, Name = "Punjab", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 80, Name = "Rajasthan", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 81, Name = "Sikkim", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 82, Name = "Tamil Nadu", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 83, Name = "Tripura", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 84, Name = "Uttar Pradesh", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 85, Name = "Uttarakhand", Code = "", CountryID = 5 });
                //items.Add(new State { ID = 86, Name = "West Bengal", Code = "", CountryID = 5 });

                //items.Add(new State { ID = 87, Name = "British Columbia", Code = "", CountryID = 7 });
                //items.Add(new State { ID = 88, Name = "Manitoba", Code = "", CountryID = 7 });
                //items.Add(new State { ID = 89, Name = "New Brunswick", Code = "", CountryID = 7 });
                //items.Add(new State { ID = 90, Name = "New Foundland", Code = "", CountryID = 7 });
                //items.Add(new State { ID = 91, Name = "Northwest Territories", Code = "", CountryID = 7 });
                //items.Add(new State { ID = 92, Name = "Nova Scotia", Code = "", CountryID = 7 });
                //items.Add(new State { ID = 93, Name = "Ontario", Code = "", CountryID = 7 });
                //items.Add(new State { ID = 94, Name = "Prince Edward Island", Code = "", CountryID = 7 });
                //items.Add(new State { ID = 95, Name = "Quebec", Code = "", CountryID = 7 });
                //items.Add(new State { ID = 96, Name = "Saskatchewan", Code = "", CountryID = 7 });
                //items.Add(new State { ID = 97, Name = "Yukon Territories", Code = "", CountryID = 7 });

                //items.Add(new State { ID = 98, Name = "Baden-Württemberg", Code = "", CountryID = 10 });
                //items.Add(new State { ID = 99, Name = "Bavaria (Bayern)", Code = "", CountryID = 10 });
                //items.Add(new State { ID = 100, Name = "Berlin", Code = "", CountryID = 10 });
                //items.Add(new State { ID = 101, Name = "Brandenburg", Code = "", CountryID = 10 });
                //items.Add(new State { ID = 102, Name = "Bremen", Code = "", CountryID = 10 });
                //items.Add(new State { ID = 103, Name = "Hamburg", Code = "", CountryID = 10 });
                //items.Add(new State { ID = 104, Name = "Hesse (Hessen)", Code = "", CountryID = 10 });
                //items.Add(new State { ID = 105, Name = "Mecklenburg-Vorpommern", Code = "", CountryID = 10 });
                //items.Add(new State { ID = 106, Name = "Lower Saxony (Niedersachsen)", Code = "", CountryID = 10 });
                //items.Add(new State { ID = 107, Name = "North Rhine- Westphalia (Nordrhein-Westfalen)", Code = "", CountryID = 10 });
                //items.Add(new State { ID = 108, Name = "Rhineland-Palatinate (Rheinland-Pfalz)", Code = "", CountryID = 10 });
                //items.Add(new State { ID = 109, Name = "Saarland", Code = "", CountryID = 10 });
                //items.Add(new State { ID = 110, Name = "Saxony (Sachsen)", Code = "", CountryID = 10 });
                //items.Add(new State { ID = 111, Name = "Saxony-Anhalt (Sachsen-Anhalt)", Code = "", CountryID = 10 });
                //items.Add(new State { ID = 112, Name = "Schleswig-Holstein", Code = "", CountryID = 10 });
                //items.Add(new State { ID = 113, Name = "Thuringia (Thüringen)", Code = "", CountryID = 10 });

                items.Add(new State { Name = "Australian Capital Territory", Code = "", CountryID = 8, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new State { Name = "New South Wales", Code = "", CountryID = 8, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new State { Name = "Northern Territory", Code = "", CountryID = 8, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new State { Name = "Queensland", Code = "", CountryID = 8, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new State { Name = "South Australia", Code = "", CountryID = 8, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new State { Name = "Tasmania", Code = "", CountryID = 8, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new State { Name = "Victoria", Code = "", CountryID = 8, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new State { Name = "Western Australia", Code = "", CountryID = 8, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                return items;
            }
        }

        public static List<City> Cities
        {
            get
            {
                List<City> items = new List<City>();

                items.Add(new City { ID = 1, Name = "Milwaukee", StateID = 49, CountryID = 1 });
                items.Add(new City { ID = 2, Name = "Madison", StateID = 49, CountryID = 1 });
                items.Add(new City { ID = 3, Name = "Green Bay", StateID = 49, CountryID = 1 });

                items.Add(new City { ID = 4, Name = "Seattle", StateID = 48, CountryID = 1 });
                items.Add(new City { ID = 5, Name = "Tacoma", StateID = 48, CountryID = 1 });
                items.Add(new City { ID = 6, Name = "Vancouver", StateID = 48, CountryID = 1 });
                items.Add(new City { ID = 7, Name = "Spokane", StateID = 48, CountryID = 1 });
                items.Add(new City { ID = 8, Name = "Everett", StateID = 48, CountryID = 1 });
                items.Add(new City { ID = 9, Name = "Bellevue", StateID = 48, CountryID = 1 });

                items.Add(new City { ID = 10, Name = "Norfolk", StateID = 47, CountryID = 1 });
                items.Add(new City { ID = 11, Name = "Newport News", StateID = 47, CountryID = 1 });
                items.Add(new City { ID = 12, Name = "Richmond", StateID = 47, CountryID = 1 });
                items.Add(new City { ID = 13, Name = "Virginia Beach", StateID = 47, CountryID = 1 });
                items.Add(new City { ID = 14, Name = "Hampton", StateID = 47, CountryID = 1 });
                items.Add(new City { ID = 15, Name = "Chesapeake", StateID = 47, CountryID = 1 });
                items.Add(new City { ID = 16, Name = "Arlington", StateID = 47, CountryID = 1 });
                items.Add(new City { ID = 17, Name = "Alexandria", StateID = 47, CountryID = 1 });

                items.Add(new City { ID = 18, Name = "Provo", StateID = 45, CountryID = 1 });
                items.Add(new City { ID = 19, Name = "West Valley City", StateID = 45, CountryID = 1 });
                items.Add(new City { ID = 20, Name = "West Jordan", StateID = 45, CountryID = 1 });
                items.Add(new City { ID = 21, Name = "Salt Lake City", StateID = 45, CountryID = 1 });

                items.Add(new City { ID = 22, Name = "McAllen", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 23, Name = "McKinney", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 24, Name = "Lubbock", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 25, Name = "Midland", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 26, Name = "Mesquite", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 27, Name = "Plano", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 28, Name = "Pasadena", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 29, Name = "Wichita Falls", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 30, Name = "Waco", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 31, Name = "San Antonio", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 32, Name = "Garland", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 33, Name = "Frisco", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 34, Name = "Fort Worth", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 35, Name = "Grand Prairie", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 36, Name = "El Paso", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 37, Name = "Killeen", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 38, Name = "Laredo", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 39, Name = "Houston", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 40, Name = "Irving", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 41, Name = "Carrollton", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 42, Name = "Denton", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 43, Name = "Corpus Christi", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 44, Name = "Dallas", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 45, Name = "Austin", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 46, Name = "Beaumont", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 47, Name = "Brownsville", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 48, Name = "Arlington", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 49, Name = "Amarillo", StateID = 44, CountryID = 1 });
                items.Add(new City { ID = 50, Name = "Abilene", StateID = 44, CountryID = 1 });

                items.Add(new City { ID = 51, Name = "Memphis", StateID = 43, CountryID = 1 });
                items.Add(new City { ID = 52, Name = "Nashville ", StateID = 43, CountryID = 1 });
                items.Add(new City { ID = 53, Name = "Murfreesboro", StateID = 43, CountryID = 1 });
                items.Add(new City { ID = 54, Name = "Knoxville", StateID = 43, CountryID = 1 });
                items.Add(new City { ID = 55, Name = "Chattanooga", StateID = 43, CountryID = 1 });
                items.Add(new City { ID = 56, Name = "Clarksville", StateID = 43, CountryID = 1 });

                items.Add(new City { ID = 57, Name = "Sioux Falls", StateID = 42, CountryID = 1 });

                items.Add(new City { ID = 58, Name = "Charleston", StateID = 41, CountryID = 1 });
                items.Add(new City { ID = 59, Name = "Columbia", StateID = 41, CountryID = 1 });

                items.Add(new City { ID = 60, Name = "Providence", StateID = 40, CountryID = 1 });

                items.Add(new City { ID = 61, Name = "Philadelphia", StateID = 39, CountryID = 1 });
                items.Add(new City { ID = 62, Name = "Pittsburgh", StateID = 39, CountryID = 1 });
                items.Add(new City { ID = 63, Name = "Erie", StateID = 39, CountryID = 1 });
                items.Add(new City { ID = 64, Name = "Allentown", StateID = 39, CountryID = 1 });

                items.Add(new City { ID = 65, Name = "Portland", StateID = 38, CountryID = 1 });
                items.Add(new City { ID = 66, Name = "Salem", StateID = 38, CountryID = 1 });
                items.Add(new City { ID = 67, Name = "Eugene", StateID = 38, CountryID = 1 });
                items.Add(new City { ID = 68, Name = "Gresham", StateID = 38, CountryID = 1 });

                items.Add(new City { ID = 69, Name = "Norman", StateID = 37, CountryID = 1 });
                items.Add(new City { ID = 70, Name = "Oklahoma City", StateID = 37, CountryID = 1 });
                items.Add(new City { ID = 71, Name = "Tulsa", StateID = 37, CountryID = 1 });

                items.Add(new City { ID = 72, Name = "Toledo", StateID = 36, CountryID = 1 });
                items.Add(new City { ID = 73, Name = "Cleveland", StateID = 36, CountryID = 1 });
                items.Add(new City { ID = 74, Name = "Cincinnati", StateID = 36, CountryID = 1 });
                items.Add(new City { ID = 75, Name = "Dayton", StateID = 36, CountryID = 1 });
                items.Add(new City { ID = 76, Name = "Columbus", StateID = 36, CountryID = 1 });
                items.Add(new City { ID = 77, Name = "Akron", StateID = 36, CountryID = 1 });

                items.Add(new City { ID = 78, Name = "Fargo", StateID = 29, CountryID = 1 });

                items.Add(new City { ID = 79, Name = "Raleigh", StateID = 28, CountryID = 1 });
                items.Add(new City { ID = 80, Name = "Winston-Salem", StateID = 28, CountryID = 1 });
                items.Add(new City { ID = 81, Name = "Wilmington", StateID = 28, CountryID = 1 });
                items.Add(new City { ID = 82, Name = "Durham", StateID = 28, CountryID = 1 });
                items.Add(new City { ID = 83, Name = "Fayetteville", StateID = 28, CountryID = 1 });
                items.Add(new City { ID = 84, Name = "Greensboro", StateID = 28, CountryID = 1 });
                items.Add(new City { ID = 85, Name = "High Point", StateID = 28, CountryID = 1 });
                items.Add(new City { ID = 86, Name = "Cary", StateID = 28, CountryID = 1 });
                items.Add(new City { ID = 87, Name = "Charlotte", StateID = 28, CountryID = 1 });

                items.Add(new City { ID = 88, Name = "New York", StateID = 35, CountryID = 1 });
                items.Add(new City { ID = 89, Name = "Syracuse", StateID = 35, CountryID = 1 });
                items.Add(new City { ID = 90, Name = "Yonkers", StateID = 35, CountryID = 1 });
                items.Add(new City { ID = 91, Name = "Rochester", StateID = 35, CountryID = 1 });
                items.Add(new City { ID = 92, Name = "Buffalo", StateID = 35, CountryID = 1 });

                items.Add(new City { ID = 93, Name = "Albuquerque", StateID = 33, CountryID = 1 });

                items.Add(new City { ID = 94, Name = "Newark", StateID = 32, CountryID = 1 });
                items.Add(new City { ID = 95, Name = "Paterson", StateID = 32, CountryID = 1 });
                items.Add(new City { ID = 96, Name = "Elizabeth", StateID = 32, CountryID = 1 });
                items.Add(new City { ID = 97, Name = "Jersey City", StateID = 32, CountryID = 1 });

                items.Add(new City { ID = 98, Name = "Manchester", StateID = 31, CountryID = 1 });

                items.Add(new City { ID = 99, Name = "North Las Vegas", StateID = 34, CountryID = 1 });
                items.Add(new City { ID = 100, Name = "Reno", StateID = 34, CountryID = 1 });
                items.Add(new City { ID = 101, Name = "Las Vegas", StateID = 34, CountryID = 1 });
                items.Add(new City { ID = 102, Name = "Henderson", StateID = 34, CountryID = 1 });

                items.Add(new City { ID = 103, Name = "Omaha", StateID = 30, CountryID = 1 });
                items.Add(new City { ID = 104, Name = "Lincoln", StateID = 30, CountryID = 1 });

                items.Add(new City { ID = 105, Name = "Billings", StateID = 27, CountryID = 1 });

                items.Add(new City { ID = 106, Name = "Springfield", StateID = 25, CountryID = 1 });
                items.Add(new City { ID = 107, Name = "Saint Louis", StateID = 25, CountryID = 1 });
                items.Add(new City { ID = 108, Name = "Kansas City", StateID = 25, CountryID = 1 });
                items.Add(new City { ID = 109, Name = "Independence", StateID = 25, CountryID = 1 });
                items.Add(new City { ID = 110, Name = "Columbia", StateID = 25, CountryID = 1 });

                items.Add(new City { ID = 111, Name = "Jackson", StateID = 26, CountryID = 1 });

                items.Add(new City { ID = 112, Name = "Minneapolis", StateID = 24, CountryID = 1 });
                items.Add(new City { ID = 113, Name = "Rochester", StateID = 24, CountryID = 1 });
                items.Add(new City { ID = 114, Name = "Saint Paul", StateID = 24, CountryID = 1 });

                items.Add(new City { ID = 115, Name = "Warren", StateID = 23, CountryID = 1 });
                items.Add(new City { ID = 116, Name = "Sterling Heights", StateID = 23, CountryID = 1 });
                items.Add(new City { ID = 117, Name = "Flint", StateID = 23, CountryID = 1 });
                items.Add(new City { ID = 118, Name = "Grand Rapids", StateID = 23, CountryID = 1 });
                items.Add(new City { ID = 119, Name = "Lansing", StateID = 23, CountryID = 1 });
                items.Add(new City { ID = 120, Name = "Detroit", StateID = 23, CountryID = 1 });
                items.Add(new City { ID = 121, Name = "Ann Arbor", StateID = 23, CountryID = 1 });

                items.Add(new City { ID = 122, Name = "Lowell", StateID = 20, CountryID = 1 });
                items.Add(new City { ID = 123, Name = "Worcester", StateID = 20, CountryID = 1 });
                items.Add(new City { ID = 124, Name = "Springfield", StateID = 20, CountryID = 1 });
                items.Add(new City { ID = 125, Name = "Cambridge", StateID = 20, CountryID = 1 });
                items.Add(new City { ID = 126, Name = "Boston", StateID = 20, CountryID = 1 });

                items.Add(new City { ID = 127, Name = "Baltimore", StateID = 21, CountryID = 1 });

                items.Add(new City { ID = 128, Name = "New Orleans", StateID = 19, CountryID = 1 });
                items.Add(new City { ID = 129, Name = "Shreveport", StateID = 19, CountryID = 1 });
                items.Add(new City { ID = 130, Name = "Lafayette", StateID = 19, CountryID = 1 });
                items.Add(new City { ID = 131, Name = "Baton Rouge", StateID = 19, CountryID = 1 });


                items.Add(new City { ID = 132, Name = "Louisville", StateID = 18, CountryID = 1 });
                items.Add(new City { ID = 133, Name = "Lexington", StateID = 18, CountryID = 1 });

                items.Add(new City { ID = 134, Name = "Overland Park", StateID = 17, CountryID = 1 });
                items.Add(new City { ID = 135, Name = "Olathe", StateID = 17, CountryID = 1 });
                items.Add(new City { ID = 136, Name = "Topeka", StateID = 17, CountryID = 1 });
                items.Add(new City { ID = 137, Name = "Wichita", StateID = 17, CountryID = 1 });
                items.Add(new City { ID = 138, Name = "Kansas City", StateID = 17, CountryID = 1 });

                items.Add(new City { ID = 139, Name = "Cedar Rapids", StateID = 13, CountryID = 1 });
                items.Add(new City { ID = 140, Name = "Des Moines", StateID = 13, CountryID = 1 });

                items.Add(new City { ID = 141, Name = "South Bend", StateID = 16, CountryID = 1 });
                items.Add(new City { ID = 142, Name = "Evansville", StateID = 16, CountryID = 1 });
                items.Add(new City { ID = 143, Name = "Fort Wayne", StateID = 16, CountryID = 1 });
                items.Add(new City { ID = 144, Name = "Indianapolis", StateID = 16, CountryID = 1 });

                items.Add(new City { ID = 145, Name = "Naperville", StateID = 15, CountryID = 1 });
                items.Add(new City { ID = 146, Name = "Peoria", StateID = 15, CountryID = 1 });
                items.Add(new City { ID = 147, Name = "Springfield", StateID = 15, CountryID = 1 });
                items.Add(new City { ID = 148, Name = "Rockford", StateID = 15, CountryID = 1 });
                items.Add(new City { ID = 149, Name = "Elgin", StateID = 15, CountryID = 1 });
                items.Add(new City { ID = 150, Name = "Joliet", StateID = 15, CountryID = 1 });
                items.Add(new City { ID = 151, Name = "Chicago", StateID = 15, CountryID = 1 });
                items.Add(new City { ID = 152, Name = "Aurora", StateID = 15, CountryID = 1 });

                items.Add(new City { ID = 153, Name = "Boise", StateID = 14, CountryID = 1 });

                items.Add(new City { ID = 154, Name = "Honolulu", StateID = 12, CountryID = 1 });

                items.Add(new City { ID = 155, Name = "Savannah", StateID = 11, CountryID = 1 });
                items.Add(new City { ID = 156, Name = "Columbus", StateID = 11, CountryID = 1 });
                items.Add(new City { ID = 157, Name = "Athens", StateID = 11, CountryID = 1 });
                items.Add(new City { ID = 158, Name = "Augusta", StateID = 11, CountryID = 1 });
                items.Add(new City { ID = 160, Name = "Atlanta", StateID = 11, CountryID = 1 });

                items.Add(new City { ID = 161, Name = "Miramar", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 162, Name = "Miami Gardens", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 163, Name = "Miami", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 164, Name = "Port Saint Lucie", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 165, Name = "Palm Bay", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 166, Name = "Pembroke Pines", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 167, Name = "Orlando", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 168, Name = "Tallahassee", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 169, Name = "Tampa", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 170, Name = "Saint Petersburg", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 171, Name = "Fort Lauderdale", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 172, Name = "Gainesville", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 173, Name = "Jacksonville", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 174, Name = "Hollywood", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 175, Name = "Hialeah", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 176, Name = "Clearwater", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 177, Name = "Coral Springs", StateID = 10, CountryID = 1 });
                items.Add(new City { ID = 178, Name = "Cape Coral", StateID = 10, CountryID = 1 });


                items.Add(new City { ID = 179, Name = "Washington", StateID = 8, CountryID = 1 });

                items.Add(new City { ID = 180, Name = "New Haven", StateID = 7, CountryID = 1 });
                items.Add(new City { ID = 181, Name = "Waterbury", StateID = 7, CountryID = 1 });
                items.Add(new City { ID = 182, Name = "Stamford", StateID = 7, CountryID = 1 });
                items.Add(new City { ID = 184, Name = "Hartford", StateID = 7, CountryID = 1 });
                items.Add(new City { ID = 185, Name = "Bridgeport", StateID = 7, CountryID = 1 });

                items.Add(new City { ID = 186, Name = "Pueblo", StateID = 6, CountryID = 1 });
                items.Add(new City { ID = 187, Name = "Thornton", StateID = 6, CountryID = 1 });
                items.Add(new City { ID = 188, Name = "Westminster", StateID = 6, CountryID = 1 });
                items.Add(new City { ID = 189, Name = "Fort Collins", StateID = 6, CountryID = 1 });
                items.Add(new City { ID = 190, Name = "Lakewood", StateID = 6, CountryID = 1 });
                items.Add(new City { ID = 191, Name = "Centennial", StateID = 6, CountryID = 1 });
                items.Add(new City { ID = 192, Name = "Colorado Springs", StateID = 6, CountryID = 1 });
                items.Add(new City { ID = 193, Name = "Denver ", StateID = 6, CountryID = 1 });
                items.Add(new City { ID = 194, Name = "Arvada", StateID = 6, CountryID = 1 });
                items.Add(new City { ID = 195, Name = "Aurora", StateID = 6, CountryID = 1 });

                items.Add(new City { ID = 196, Name = "Los Angeles", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 197, Name = "Norwalk", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 198, Name = "Oakland", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 199, Name = "Murrieta", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 200, Name = "Moreno Valley", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 201, Name = "Modesto", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 202, Name = "Pomona", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 203, Name = "Richmond", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 204, Name = "Riverside", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 205, Name = "Rancho Cucamonga", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 206, Name = "Palmdale", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 207, Name = "Pasadena", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 208, Name = "Orange", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 209, Name = "Oxnard", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 210, Name = "Ontario", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 211, Name = "Oceanside", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 212, Name = "Sunnyvale", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 213, Name = "Temecula", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 214, Name = "Vallejo", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 215, Name = "Torrance", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 216, Name = "Thousand Oaks", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 217, Name = "West Covina", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 218, Name = "Visalia", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 219, Name = "Victorville", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 220, Name = "Santa Clara", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 221, Name = "Santa Clarita", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 222, Name = "Santa Rosa", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 223, Name = "Simi Valley", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 224, Name = "Stockton", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 225, Name = "San Buenaventura", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 226, Name = "San Bernardino", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 227, Name = "Santa Ana", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 228, Name = "San Jose", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 229, Name = "San Diego", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 230, Name = "San Francisco", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 231, Name = "Salinas", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 232, Name = "Sacramento", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 233, Name = "Roseville", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 234, Name = "El Monte", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 235, Name = "Escondido", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 236, Name = "Elk Grove", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 237, Name = "Fontana", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 238, Name = "Fairfield", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 239, Name = "Glendale", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 240, Name = "Fullerton", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 241, Name = "Garden Grove", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 242, Name = "Fresno", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 243, Name = "Fremont", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 244, Name = "Long Beach", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 245, Name = "Lancaster", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 246, Name = "Huntington Beach", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 247, Name = "Irvine", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 248, Name = "Inglewood", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 249, Name = "Hayward", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 250, Name = "Carlsbad", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 251, Name = "Chula Vista", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 252, Name = "Daly City", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 253, Name = "Downey", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 254, Name = "Corona", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 255, Name = "Costa Mesa", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 256, Name = "Concord", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 257, Name = "Bakersfield", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 258, Name = "Berkeley", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 259, Name = "Burbank", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 260, Name = "Antioch", StateID = 5, CountryID = 1 });
                items.Add(new City { ID = 261, Name = "Anaheim", StateID = 5, CountryID = 1 });

                items.Add(new City { ID = 262, Name = "Little Rock", StateID = 3, CountryID = 1 });

                items.Add(new City { ID = 263, Name = "Mesa", StateID = 4, CountryID = 1 });
                items.Add(new City { ID = 264, Name = "Phoenix", StateID = 4, CountryID = 1 });
                items.Add(new City { ID = 265, Name = "Peoria", StateID = 4, CountryID = 1 });
                items.Add(new City { ID = 266, Name = "Surprise", StateID = 4, CountryID = 1 });
                items.Add(new City { ID = 267, Name = "Tempe", StateID = 4, CountryID = 1 });
                items.Add(new City { ID = 268, Name = "Tucson", StateID = 4, CountryID = 1 });
                items.Add(new City { ID = 269, Name = "Scottsdale", StateID = 4, CountryID = 1 });
                items.Add(new City { ID = 270, Name = "Gilbert", StateID = 4, CountryID = 1 });
                items.Add(new City { ID = 271, Name = "Glendale", StateID = 4, CountryID = 1 });
                items.Add(new City { ID = 272, Name = "Chandler", StateID = 4, CountryID = 1 });

                items.Add(new City { ID = 273, Name = "Anchorage", StateID = 1, CountryID = 1 });

                items.Add(new City { ID = 274, Name = "Mobile", StateID = 2, CountryID = 1 });
                items.Add(new City { ID = 275, Name = "Montgomery", StateID = 2, CountryID = 1 });
                items.Add(new City { ID = 276, Name = "Huntsville", StateID = 2, CountryID = 1 });
                items.Add(new City { ID = 277, Name = "Birmingham", StateID = 2, CountryID = 1 });
                items.Add(new City { ID = 278, Name = "SAINT ALBANS", StateID = 35, CountryID = 1 });

                //SAINT ALBANS

                return items;
            }
        }

        public static List<Gender> Genders
        {
            get
            {
                List<Gender> items = new List<Gender>();
                items.Add(new Gender { ID = 1, Name = "Female", Description = "Female", IsActive = true });
                items.Add(new Gender { ID = 2, Name = "Male", Description = "Male", IsActive = true });
                items.Add(new Gender { ID = 3, Name = "Transgender", Description = "Transgender", IsActive = true });
                return items;
            }
        }

        public static List<EmailTemplateCategory> EmailTemplateCategory
        {
            get
            {
                List<EmailTemplateCategory> items = new List<EmailTemplateCategory>();
                items.Add(new EmailTemplateCategory { Name = "Forget Password", Description = "Forget Password Email Category", IsActive = true });
                return items;
            }
        }

        public static List<EmailTemplate> EmailTemplate
        {
            get
            {
                string emailBody = @"Dear User,<br /><br />Here is your eCMS account access details.<br /><br />User Name:[Worker.LoginName]<br />Password:[Worker.Password]<br /><br />Regards,<br /><br />eCMS System Manager";
                List<EmailTemplate> items = new List<EmailTemplate>();
                items.Add(new EmailTemplate
                {
                    Name = "Forget Password",
                    CreateDate = DateTime.Now,
                    CreatedByWorkerID = 1,
                    EmailBody = emailBody,
                    EmailSubject = "Forget Password",
                    EmailTemplateCategoryID = 1,
                    LastUpdateDate = DateTime.Now,
                    IsArchived = false,
                    LastUpdatedByWorkerID = 1
                });
                return items;
            }
        }

        public static List<ActionStatus> ActionStatus
        {
            get
            {
                List<ActionStatus> items = new List<ActionStatus>();
                items.Add(new ActionStatus { Name = "Not Started", Description = "Not Started", IsActive=true });
                items.Add(new ActionStatus { Name = "Completed", Description = "Completed", IsActive = true });
                items.Add(new ActionStatus { Name = "Withdrawn", Description = "Withdrawn", IsActive = true });
                return items;
            }
        }

        public static List<ActivityType> ActivityType
        {
            get
            {
                List<ActivityType> items = new List<ActivityType>();
                items.Add(new ActivityType { Name = "Financial Hardship", Description = "Financial Hardship", IsActive = true });
                items.Add(new ActivityType { Name = "Intake", Description = "Intake", IsActive = true });
                items.Add(new ActivityType { Name = "Assessment", Description = "Assessment", IsActive = true });
                items.Add(new ActivityType { Name = "Support", Description = "Support", IsActive = true });
                items.Add(new ActivityType { Name = "Case Management", Description = "Case Management", IsActive = true });
                items.Add(new ActivityType { Name = "Crisis Management", Description = "Crisis Management", IsActive = true });
                items.Add(new ActivityType { Name = "Funding Request", Description = "Funding Request", IsActive = true });
                return items;
            }
        }

        public static List<ContactMethod> ContactMethod
        {
            get
            {
                List<ContactMethod> items = new List<ContactMethod>();
                items.Add(new ContactMethod { Name = "Member - In Person", Description = "Member - In Person", IsActive = true });
                items.Add(new ContactMethod { Name = "Member - Telephone", Description = "Member - Telephone", IsActive = true });
                items.Add(new ContactMethod { Name = "Member - Electronic", Description = "Member - Electronic", IsActive = true });
                items.Add(new ContactMethod { Name = "Third Party - In Person", Description = "Third Party - In Person", IsActive = true });
                items.Add(new ContactMethod { Name = "Third Party - Telephone", Description = "Third Party - Telephone", IsActive = true });
                items.Add(new ContactMethod { Name = "Third Party - Electronic", Description = "Third Party - Electronic", IsActive = true });
                return items;
            }
        }

        public static List<ContactMedia> ContactMedia
        {
            get
            {
                List<ContactMedia> items = new List<ContactMedia>();
                items.Add(new ContactMedia { Name = "Email", Description = "Email", IsActive = true });
                items.Add(new ContactMedia { Name = "Home", Description = "Home", IsActive = true });
                items.Add(new ContactMedia { Name = "Mobile", Description = "Mobile", IsActive = true });
                items.Add(new ContactMedia { Name = "Office", Description = "Office", IsActive = true });
                items.Add(new ContactMedia { Name = "Work", Description = "Work", IsActive = true });
                return items;
            }
        }

        public static List<Ethnicity> Ethnicity
        {
            get
            {
                List<Ethnicity> items = new List<Ethnicity>();
                items.Add(new Ethnicity { Name = "Afghan", Description = "Afghan", IsActive = true });
                items.Add(new Ethnicity { Name = "Indian", Description = "Indian", IsActive = true });
                items.Add(new Ethnicity { Name = "Pakistani", Description = "Pakistani", IsActive = true });
                items.Add(new Ethnicity { Name = "Iranian", Description = "Iranian", IsActive = true });
                items.Add(new Ethnicity { Name = "East African", Description = "East African", IsActive = true });
                items.Add(new Ethnicity { Name = "Canadian", Description = "Canadian", IsActive = true });
                items.Add(new Ethnicity { Name = "Other", Description = "Other", IsActive = true });
                items.Add(new Ethnicity { Name = "Syrian", Description = "Syrian", IsActive = true });
                items.Add(new Ethnicity { Name = "Tajik", Description = "Tajik", IsActive = true });
                items.Add(new Ethnicity { Name = "Bangladeshi", Description = "Bangladeshi", IsActive = true });
                items.Add(new Ethnicity { Name = "Third Generation", Description = "Third Generation", IsActive = true });
                return items;
            }
        }

        public static List<CaseStatus> CaseStatus
        {
            get
            {
                List<CaseStatus> items = new List<CaseStatus>();
                items.Add(new CaseStatus { Name = "Active - In progress", Description = "Active - In progress", IsActive = true });
                items.Add(new CaseStatus { Name = "Active - Onboarding", Description = "Active - Onboarding", IsActive = true });
                items.Add(new CaseStatus { Name = "Hold", Description = "Hold", IsActive = true });
                items.Add(new CaseStatus { Name = "Monitoring - Family not ready", Description = "Monitoring - Family not ready", IsActive = true });
                items.Add(new CaseStatus { Name = "Monitoring - Completed", Description = "Monitoring - Completed", IsActive = true });
                items.Add(new CaseStatus { Name = "Monitoring - Referred to external agency", Description = "Monitoring - Referred to external agency", IsActive = true });
                items.Add(new CaseStatus { Name = "Closed - Family Declined Case Plan", Description = "Closed - Family Declined Case Plan", IsActive = true });
                items.Add(new CaseStatus { Name = "Closed - External Agency Fulfilled", Description = "Closed - External Agency Fulfilled", IsActive = true });
                items.Add(new CaseStatus { Name = "Closed - Completed", Description = "Closed - Completed", IsActive = true });
                items.Add(new CaseStatus { Name = "Closed - Not Qualified", Description = "Closed - Not Qualified", IsActive = true });
                items.Add(new CaseStatus { Name = "Closed - Relocated/Death", Description = "Closed - Relocated/Death", IsActive = true });
                items.Add(new CaseStatus { Name = "Closed - Unsuccesful Completion", Description = "Closed - Unsuccesful Completion", IsActive = true });
                items.Add(new CaseStatus { Name = "Closed - Lack of Family Engagement", Description = "Closed - Lack of Family Engagement", IsActive = true });
                items.Add(new CaseStatus { Name = "Closed - Family Withdrew", Description = "Closed - Family Withdrew", IsActive = true });
                return items;
            }
        }

        public static List<HearingSource> HearingSource
        {
            get
            {
                List<HearingSource> items = new List<HearingSource>();
                items.Add(new HearingSource { Name = "QLIP - Quality of Life Improvement", Description = "QLIP - Quality of Life Improvement", IsActive = true });
                items.Add(new HearingSource { Name = "Regional Council", Description = "Regional Council", IsActive = true });
                items.Add(new HearingSource { Name = "Self", Description = "Self", IsActive = true });
                items.Add(new HearingSource { Name = "Self Referral", Description = "Self Referral", IsActive = true });
                items.Add(new HearingSource { Name = "Settlement", Description = "Settlement", IsActive = true });
                items.Add(new HearingSource { Name = "SSS", Description = "SSS", IsActive = true });
                items.Add(new HearingSource { Name = "STL - Settlement Board", Description = "STL - Settlement Board", IsActive = true });
                items.Add(new HearingSource { Name = "SWB - Social Welfare Board", Description = "SWB - Social Welfare Board", IsActive = true });
                items.Add(new HearingSource { Name = "YSB - Youth and Sports Board", Description = "YSB - Youth and Sports Board", IsActive = true });
                items.Add(new HearingSource { Name = "Al-Akhbar", Description = "Al-Akhbar", IsActive = true });
                items.Add(new HearingSource { Name = "Family", Description = "Family", IsActive = true });
                items.Add(new HearingSource { Name = "Friends", Description = "Friends", IsActive = true });
                items.Add(new HearingSource { Name = "M/K Leadership", Description = "M/K Leadership", IsActive = true });
                items.Add(new HearingSource { Name = "Other", Description = "Other", IsActive = true });
                items.Add(new HearingSource { Name = "IICanada Portal", Description = "IICanada Portal", IsActive = true });
                items.Add(new HearingSource { Name = "Jamati Announcement", Description = "Jamati Announcement", IsActive = true });
                return items;
            }
        }

        public static List<Language> Language
        {
            get
            {
                List<Language> items = new List<Language>();
                items.Add(new Language { Name = "Chitrali", Description = "Chitrali", IsActive = true });
                items.Add(new Language { Name = "Dari", Description = "Dari", IsActive = true });
                items.Add(new Language { Name = "English", Description = "English", IsActive = true });
                items.Add(new Language { Name = "Farsi", Description = "Farsi", IsActive = true });
                items.Add(new Language { Name = "French", Description = "French", IsActive = true });
                items.Add(new Language { Name = "Gujarati", Description = "Gujarati", IsActive = true });
                items.Add(new Language { Name = "Hindi", Description = "Hindi", IsActive = true });
                items.Add(new Language { Name = "Katchi", Description = "Katchi", IsActive = true });
                items.Add(new Language { Name = "Other", Description = "Other", IsActive = true });
                items.Add(new Language { Name = "Pashto / Pasai", Description = "Pashto / Pasai", IsActive = true });
                items.Add(new Language { Name = "Russian", Description = "Russian", IsActive = true });
                items.Add(new Language { Name = "Urdu", Description = "Urdu", IsActive = true });
                items.Add(new Language { Name = "Tajik", Description = "Tajik", IsActive = true });
                items.Add(new Language { Name = "Arabic", Description = "Arabic", IsActive = true });
                return items;
            }
        }

        public static List<MaritalStatus> MaritalStatus
        {
            get
            {
                List<MaritalStatus> items = new List<MaritalStatus>();
                items.Add(new MaritalStatus { Name = "Common Law", Description = "Common Law", IsActive = true });
                items.Add(new MaritalStatus { Name = "Divorced", Description = "Divorced", IsActive = true });
                items.Add(new MaritalStatus { Name = "Married", Description = "Married", IsActive = true });
                items.Add(new MaritalStatus { Name = "Separated", Description = "Separated", IsActive = true });
                items.Add(new MaritalStatus { Name = "Single, Never Married", Description = "Single, Never Married", IsActive = true });
                items.Add(new MaritalStatus { Name = "Widowed", Description = "Widowed", IsActive = true });
                return items;
            }
        }

        public static List<MemberStatus> MemberStatus
        {
            get
            {
                List<MemberStatus> items = new List<MemberStatus>();
                items.Add(new MemberStatus { Name = "Active - In progress", Description = "Active - In progress", IsActive = true });
                items.Add(new MemberStatus { Name = "Active - Onboarding", Description = "Active - Onboarding", IsActive = true });
                items.Add(new MemberStatus { Name = "Hold", Description = "Hold", IsActive = true });
                items.Add(new MemberStatus { Name = "Monitoring - Managed by External Agency", Description = "Monitoring - Managed by External Agency", IsActive = true });
                items.Add(new MemberStatus { Name = "Monitoring - Completed", Description = "Monitoring - Completed", IsActive = true });
                items.Add(new MemberStatus { Name = "Monitoring - Not Ready", Description = "Monitoring - Not Ready", IsActive = true });
                items.Add(new MemberStatus { Name = "Monitoring - General", Description = "Monitoring - General", IsActive = true });
                items.Add(new MemberStatus { Name = "Inactive - Lack of Engagement", Description = "Inactive - Lack of Engagement", IsActive = true });
                items.Add(new MemberStatus { Name = "Inactive - Relocated", Description = "Inactive - Relocated", IsActive = true });
                items.Add(new MemberStatus { Name = "Inactive - Other", Description = "Inactive - Other", IsActive = true });
                items.Add(new MemberStatus { Name = "Inactive - In Crisis", Description = "Inactive - In Crisis", IsActive = true });
                items.Add(new MemberStatus { Name = "Inactive - Declined Participation", Description = "Inactive - Declined Participation", IsActive = true });
                items.Add(new MemberStatus { Name = "Inactive - Withdrew", Description = "Inactive - Withdrew", IsActive = true });
                return items;
            }
        }

        public static List<Program> Program
        {
            get
            {
                List<Program> items = new List<Program>();
                items.Add(new Program { Name = "eCMS", Description = "eCMS", IsActive = true });
                items.Add(new Program { Name = "Other", Description = "Other", IsActive = true });
                return items;
            }
        }

        public static List<SubProgram> SubProgram
        {
            get
            {
                List<SubProgram> items = new List<SubProgram>(); 
                items.Add(new SubProgram { Name = "SSS", Description = "Social Support Services (SWB)", IsActive = true, ProgramBase = "M", ProgramEmail = true, ProgramMember = "I", ProgramID = 1, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SubProgram { Name = "QOL (Office)", Description = "QOL (Office)", IsActive = true, ProgramBase = "F", ProgramEmail = true, ProgramMember = "I", ProgramID = 1, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SubProgram { Name = "QOL (JK)", Description = "QOL (JK)", IsActive = false, ProgramBase = "M", ProgramEmail = false, ProgramMember = "I", ProgramID = 1, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                return items;
            }
        }

        public static List<ReferralSource> ReferralSource
        {
            get
            {
                List<ReferralSource> items = new List<ReferralSource>();
                
                items.Add(new ReferralSource { Name = "CAB - Conciliation and Arbitration Board", Description = "CAB - Conciliation and Arbitration Board", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "EB - Education Board", Description = "EB - Education Board", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "Engaged Family", Description = "Engaged Family", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "EPB - Economic Planning Board", Description = "EPB - Economic Planning Board", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "Family & Friends", Description = "Family & Friends", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "HB - Health Board", Description = "HB - Health Board", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "Other Institution", Description = "Other Institution", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "ITREB - Tariqah Board", Description = "ITREB - Tariqah Board", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "IVC Leadership", Description = "IVC Leadership", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "Jamati Member", Description = "Jamati Member", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "Jamati Leadership", Description = "Jamati Leadership", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "Institutional Leadership", Description = "Institutional Leadership", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "MKs - Jamati MukhiKamadia", Description = "MKs - Jamati MukhiKamadia", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "SWB - Social Welfare Board", Description = "SWB - Social Welfare Board", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "YSB - Youth & Sports Board", Description = "YSB - Youth & Sports Board", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "Previously Engaged Family", Description = "Previously Engaged Family", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "QLIP Transfer", Description = "QLIP Transfer", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "RIPI Transfer", Description = "RIPI Transfer", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "Settlement", Description = "Settlement", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "Self", Description = "Self", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "Other", Description = "Other", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ReferralSource { Name = "IICanada Portal", Description = "IICanada Portal", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                return items;
            }
        }

        public static List<Region> Region
        {
            get
            {
                List<Region> items = new List<Region>();
                items.Add(new Region { Code = "B.C.", Name = "B.C.", Description = "British Columbia", CountryID = 7, IsActive = true });
                items.Add(new Region { Code = "Edmonton", Name = "Edmonton", Description = "National", CountryID = 7, IsActive = true });
                items.Add(new Region { Code = "National", Name = "National", Description = "Edmonton", CountryID = 7, IsActive = true });
                items.Add(new Region { Code = "Ontario", Name = "Ontario", Description = "Ontario", CountryID = 7, IsActive = true });
                items.Add(new Region { Code = "Ottawa", Name = "Ottawa", Description = "Ottawa", CountryID = 7, IsActive = true });
                items.Add(new Region { Code = "Prairies", Name = "Prairies", Description = "Prairies", CountryID = 7, IsActive = true });
                items.Add(new Region { Code = "Quebec & Maritimes", Name = "Quebec & Maritimes", Description = "Quebec & Maritimes", CountryID = 7, IsActive = true });
                return items;
            }
        }

        public static List<IntakeMethod> IntakeMethod
        {
            get
            {
                List<IntakeMethod> items = new List<IntakeMethod>();
                items.Add(new IntakeMethod { Name = "Phone 1-888 Line", Description = "Phone 1-888 Line", IsActive = true });
                items.Add(new IntakeMethod { Name = "Phone - SSP", Description = "Phone - SSP", IsActive = true });
                items.Add(new IntakeMethod { Name = "Walk-in", Description = "Walk-in", IsActive = true });
                items.Add(new IntakeMethod { Name = "ICARE", Description = "ICARE", IsActive = true });
                items.Add(new IntakeMethod { Name = "Other", Description = "Other", IsActive = true });
                items.Add(new IntakeMethod { Name = "JK Lead", Description = "JK Lead", IsActive = true });
                return items;
            }
        }

        public static List<WorkerRolePermission> WorkerRolePermission
        {
            get
            {
                List<WorkerRolePermission> items = new List<WorkerRolePermission>();
                items.Add(new WorkerRolePermission { WorkerRoleID = 1, Permission = 1, IsActive = true });

                items.Add(new WorkerRolePermission { WorkerRoleID = 2, Permission = 1, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 2, Permission = 2, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 2, Permission = 3, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 2, Permission = 4, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 2, Permission = 5, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 2, Permission = 6, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 2, Permission = 7, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 2, Permission = 8, IsActive = true });

                items.Add(new WorkerRolePermission { WorkerRoleID = 3, Permission = 1, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 3, Permission = 2, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 3, Permission = 3, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 3, Permission = 4, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 3, Permission = 5, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 3, Permission = 6, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 3, Permission = 7, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 3, Permission = 8, IsActive = true });

                items.Add(new WorkerRolePermission { WorkerRoleID = 4, Permission = 2, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 4, Permission = 4, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 4, Permission = 6, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 4, Permission = 8, IsActive = true });

                items.Add(new WorkerRolePermission { WorkerRoleID = 5, Permission = 2, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 5, Permission = 4, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 5, Permission = 6, IsActive = true });
                items.Add(new WorkerRolePermission { WorkerRoleID = 5, Permission = 8, IsActive = true });
                
                return items;
            }
        }

        public static List<HighestLevelOfEducation> HighestLevelOfEducation
        {
            get
            {
                List<HighestLevelOfEducation> items = new List<HighestLevelOfEducation>();
                items.Add(new HighestLevelOfEducation { Name = "No Education", Description = "No Education", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "Grade 1", Description = "Grade 1", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "Grade 2", Description = "Grade 2", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "Grade 3", Description = "Grade 3", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "Grade 4", Description = "Grade 4", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "Grade 5", Description = "Grade 5", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "Grade 6", Description = "Grade 6", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "Grade 7", Description = "Grade 7", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "Grade 8", Description = "Grade 8", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "Grade 9", Description = "Grade 9", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "Grade 10", Description = "Grade 10", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "Grade 11", Description = "Grade 11", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "Grade 12", Description = "Grade 12", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "Grade 13", Description = "Grade 13", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "GED", Description = "GED", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "Apprenticeship or Trades Certificate or Diploma", Description = "Apprenticeship or Trades Certificate or Diploma", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "Technical/ Institute Graduate", Description = "Technical/ Institute Graduate", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "University Graduate (Bachelors)", Description = "University Graduate (Bachelors)", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "University Post-graduate (Masters)", Description = "University Post-graduate (Masters)", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "University Post-graduate (e.g. PhD, MD, LLB)", Description = "University Post-graduate (e.g. PhD, MD, LLB)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new HighestLevelOfEducation { Name = "Other", Description = "Other", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                return items;
            }
        }

        public static List<GPA> GPA
        {
            get
            {
                List<GPA> items = new List<GPA>();
                items.Add(new GPA { Name = "0-24", Description = "0-24", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new GPA { Name = "25-49", Description = "25-49", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new GPA { Name = "50-59", Description = "50-59", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new GPA { Name = "60-69", Description = "60-69", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new GPA { Name = "70-79", Description = "70-79", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new GPA { Name = "80 and Above", Description = "80 and Above", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                return items;
            }
        }

        public static List<AnnualIncome> AnnualIncome
        {
            get
            {
                List<AnnualIncome> items = new List<AnnualIncome>();
                items.Add(new AnnualIncome { Name = "Unknown or Needs Help Assessing Income", Description = "Unknown or Needs Help Assessing Income", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new AnnualIncome { Name = "Less than $5,000", Description = "Less than $5,000", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new AnnualIncome { Name = "$5,000 – $9,999", Description = "$5,000 – $9,999", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new AnnualIncome { Name = "$10,000 – $14,999", Description = "$10,000 – $14,999", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new AnnualIncome { Name = "$15,000 – $19,999", Description = "$15,000 – $19,999", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new AnnualIncome { Name = "$20,000 – $24,999", Description = "$20,000 – $24,999", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new AnnualIncome { Name = "$25,000 – $34,999", Description = "$25,000 – $34,999", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new AnnualIncome { Name = "$35,000 – $39,999", Description = "$35,000 – $39,999", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new AnnualIncome { Name = "$40,000 – $44,999", Description = "$40,000 – $44,999", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new AnnualIncome { Name = "$45,000 – $49,999", Description = "$45,000 – $49,999", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new AnnualIncome { Name = "$50,000 – $54,999", Description = "$50,000 – $54,999", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new AnnualIncome { Name = "$55,000 – $59,999", Description = "$55,000 – $59,999", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new AnnualIncome { Name = "$60,000 – $64,999", Description = "$60,000 – $64,999", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new AnnualIncome { Name = "$65,000 – $69,999", Description = "$65,000 – $69,999", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new AnnualIncome { Name = "$70,000 – $74,999", Description = "$70,000 – $74,999", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new AnnualIncome { Name = "$75,000 – $79,999", Description = "$75,000 – $79,999", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new AnnualIncome { Name = "$80,000 – $84,999", Description = "$80,000 – $84,999", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new AnnualIncome { Name = "$85,000 and Above", Description = "$85,000 and Above", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                return items;
            }
        }

        public static List<Savings> Savings
        {
            get
            {
                List<Savings> items = new List<Savings>();
                items.Add(new Savings { Name = "Unknown  or Needs Help Assessing Income", Description = "Unknown  or Needs Help Assessing Income", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Savings { Name = "Less than $5,000", Description = "Less than $5,000", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new Savings { Name = "$5,000 – $9,999", Description = "$5,000 – $9,999", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new Savings { Name = "$10,000 – $14,999", Description = "$10,000 – $14,999", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new Savings { Name = "$15,000 – $19,999", Description = "$15,000 – $19,999", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new Savings { Name = "$20,000 – $24,999", Description = "$20,000 – $24,999", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new Savings { Name = "$25,000 – $34,999", Description = "$25,000 – $34,999", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new Savings { Name = "$35,000 – $39,999", Description = "$35,000 – $39,999", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new Savings { Name = "$40,000 – $49,999", Description = "$40,000 – $49,999", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Savings { Name = "$50,000 – $74,999", Description = "$50,000 – $74,999", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Savings { Name = "$75,000 – $99,999", Description = "$75,000 – $99,999", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Savings { Name = "$100,000 – $149,999", Description = "$100,000 – $149,999", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Savings { Name = "$150,000 – $199,999", Description = "$150,000 – $199,999", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Savings { Name = "$200,000 and Above", Description = "$200,000 and Above", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                return items;
            }
        }

        public static List<ServiceProvider> ExternalServiceProvider
        {
            get
            {
                List<ServiceProvider> items = new List<ServiceProvider>();
                items.Add(new ServiceProvider { RegionID = 2, Name = "Quest Outreach Society", Description = "Quest Outreach Society", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 2, Name = "S.U.C.C.E.S.S", Description = "S.U.C.C.E.S.S", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 2, Name = "DiverseCity", Description = "DiverseCity", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 2, Name = "UBC", Description = "UBC", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });

                items.Add(new ServiceProvider { RegionID = 6, Name = "Immigrant Services Calgary", Description = "Immigrant Services Calgary", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 6, Name = "Mount Royal University, Faculty of Social Work", Description = "Mount Royal University, Faculty of Social Work", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 6, Name = "Calgary Police Services", Description = "Calgary Police Services", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });

                items.Add(new ServiceProvider { RegionID = 3, Name = "Edmonton support network BC - Quest Outreach Society", Description = "Edmonton support network BC - Quest Outreach Society", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 3, Name = "Edmonton support network", Description = "Edmonton support network", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 3, Name = "Big brothers/ big sisters", Description = "Big brothers/ big sisters", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 3, Name = "Edmonton Mennonite centre", Description = "Edmonton Mennonite centre", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });

                items.Add(new ServiceProvider { RegionID = 4, Name = "ACCES Employment", Description = "ACCES Employment", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Accreditation Assistance Access Centre", Description = "Accreditation Assistance Access Centre", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "BMO Financial Group Human Resources", Description = "BMO Financial Group Human Resources", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Burlington Employment Resource Centre", Description = "Burlington Employment Resource Centre", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Catholic Immigration Centre of Ottawa (CIC). CIC is Settlement NGO.", Description = "Catholic Immigration Centre of Ottawa (CIC). CIC is Settlement NGO.", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "City of Toronto Social Services", Description = "City of Toronto Social Services", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Computer Skills Training at YMCA", Description = "Computer Skills Training at YMCA", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "COSTI Agency", Description = "COSTI Agency", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Danforth Assessment Centre", Description = "Danforth Assessment Centre", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Dixon Hall", Description = "Dixon Hall", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Employer Marketing and Outreach Consultant at Access Employment", Description = "Employer Marketing and Outreach Consultant at Access Employment", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Employment Services - ACE", Description = "Employment Services - ACE", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Enterprise Toronto", Description = "Enterprise Toronto", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Health education and programming delivered with the social service network.", Description = "Health education and programming delivered with the social service network.", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Immigrant Women Services Ottawa (IWSO)", Description = "Immigrant Women Services Ottawa (IWSO)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Job Search Workshop Facilitator", Description = "Job Search Workshop Facilitator", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Lawyer at Thorncliffe Neighbourhood", Description = "Lawyer at Thorncliffe Neighbourhood", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Local Agencies Service Immigrants World Skills (LASI)", Description = "Local Agencies Service Immigrants World Skills (LASI)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Ministry of Children and Youth Services", Description = "Ministry of Children and Youth Services", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Ministry of Training, Colleges and Universities-Ontario", Description = "Ministry of Training, Colleges and Universities-Ontario", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Nepean, Rideau and Osgoode Community Centre (NROCRC)", Description = "Nepean, Rideau and Osgoode Community Centre (NROCRC)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "St. Stephen’s Community House", Description = "St. Stephen’s Community House", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Student Visa Department", Description = "Student Visa Department", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Thorncliffe Neighbourhood Office", Description = "Thorncliffe Neighbourhood Office", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "University Settlement Employment and Training Centre", Description = "University Settlement Employment and Training Centre", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "WoodGreen Community Service", Description = "WoodGreen Community Service", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "World Education Services Ontario", Description = "World Education Services Ontario", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "YMCA Morningside ERC & YMCA Toronto", Description = "YMCA Morningside ERC & YMCA Toronto", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "York Community Services", Description = "York Community Services", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "YWCA Toronto", Description = "YWCA Toronto", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 4, Name = "Youth Services Bureau of Ottawa (YSB)", Description = "Youth Services Bureau of Ottawa (YSB)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });

                items.Add(new ServiceProvider { RegionID = 5, Name = "The Ottawa Community Immigrant Services organization (OCISO)", Description = "The Ottawa Community Immigrant Services organization (OCISO)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 5, Name = "Pinecrest Queensway Community Health Centre (PQCHC)", Description = "Pinecrest Queensway Community Health Centre (PQCHC)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });

                items.Add(new ServiceProvider { RegionID = 7, Name = "Centre local de services communautaires (CLSC, local community service centre) – Laval,  Brossard", Description = "Centre local de services communautaires (CLSC, local community service centre) – Laval,  Brossard", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });
                items.Add(new ServiceProvider { RegionID = 7, Name = "Centre de santé et de services sociaux (CSSS, Centre of health and social services) - Montreal", Description = "Centre de santé et de services sociaux (CSSS, Centre of health and social services) - Montreal", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = true });

                return items;
            }
        }

        public static List<ServiceProvider> InternalServiceProvider
        {
            get
            {
                List<ServiceProvider> items = new List<ServiceProvider>();
                items.Add(new ServiceProvider { Name = "SWB-Social Welfare Board", Description = "SWB-Social Welfare Board", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = false });
                items.Add(new ServiceProvider { Name = "EPB-Economic Planning Board", Description = "EPB-Economic Planning Board", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = false });
                items.Add(new ServiceProvider { Name = "HB-Health Board", Description = "HB-Health Board", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = false });
                items.Add(new ServiceProvider { Name = "EB-Education Board", Description = "EB-Education Board", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = false });
                items.Add(new ServiceProvider { Name = "SP-Settlement Portfolio", Description = "SP-Settlement Portfolio", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = false });
                items.Add(new ServiceProvider { Name = "YSB-Youth and Sports Board", Description = "YSB-Youth and Sports Board", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = false });
                items.Add(new ServiceProvider { Name = "WDP-Women's Development Portfolio", Description = "WDP-Women's Development Portfolio", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = false });
                items.Add(new ServiceProvider { Name = "CAB-Conciliation and Arbitration Board", Description = "CAB-Conciliation and Arbitration Board", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = false });
                items.Add(new ServiceProvider { Name = "F.O.C.U.S", Description = "F.O.C.U.S", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now, IsExternal = false });
                return items;
            }
        }

        public static List<Service> ServiceProviderServices
        {
            get
            {
                List<Service> items = new List<Service>();
                items.Add(new Service { ServiceProviderID=1, Name = "Accreditation", Description = "Accreditation", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Administration - Forms", Description = "Administration - Forms", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Camps", Description = "Camps", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Career Planning", Description = "Career Planning", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Citizenship", Description = "Citizenship", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Counseling", Description = "Counseling", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Daycare", Description = "Daycare", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Education - Special Needs", Description = "Education - Special Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Education - ESL", Description = "Education - ESL", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Education - Skills Upgrading", Description = "Education - Skills Upgrading", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Employment", Description = "Employment", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Extra Curricular", Description = "Extra Curricular", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Family Violence", Description = "Family Violence", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Financial / Budgeting", Description = "Financial / Budgeting", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Financial - Budgeting", Description = "Financial - Budgeting", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Financial - Debt Managemen", Description = "Financial - Debt Managemen", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Financial - Employment", Description = "Financial - Employment", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Financial - Housing", Description = "Financial - Housing", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Financial - Other", Description = "Financial - Other", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Financial - Tuition / Schooling", Description = "Financial - Tuition / Schooling", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Government Benefits", Description = "Government Benefits", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Health - Check Up", Description = "Health - Check Up", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Health - Physical Health Challenges", Description = "Health - Physical Health Challenges", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Health - Emotional Instability", Description = "Health - Emotional Instability", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Health - Mental Health - Diagnosed", Description = "Health - Mental Health - Diagnosed", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Education - High School Upgrading", Description = "Education - High School Upgrading", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Housing", Description = "Housing", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Human Capital - Education Upgrading / Skills", Description = "Human Capital - Education Upgrading / Skills", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Human Capital - ESL", Description = "Human Capital - ESL", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Employment - Interview Skills", Description = "Employment - Interview Skills", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Employment - Resume Upgrading", Description = "Employment - Resume Upgrading", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Immigration", Description = "Immigration", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Education - Language Training", Description = "Education - Language Training", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Legal - Judicial", Description = "Legal - Judicial", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Legal - Paperwork", Description = "Legal - Paperwork", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Mentoring", Description = "Mentoring", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Mentorship", Description = "Mentorship", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Mobilize Funding - Govt. / External Sources", Description = "Mobilize Funding - Govt. / External Sources", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "New Arrival", Description = "New Arrival", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Parenting", Description = "Parenting", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Seniors Benefits", Description = "Seniors Benefits", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Social - Addiction (Alcohol)", Description = "Social - Addiction (Alcohol)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Social - Addictions (Drugs)", Description = "Social - Addictions (Drugs)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Social - Addictions (Gambling)", Description = "Social - Addictions (Gambling)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Social - Criminal Charges / Probation", Description = "Social - Criminal Charges / Probation", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Social - Disengaged Youth", Description = "Social - Disengaged Youth", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Social - Networking", Description = "Social - Networking", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Social - Other Violence", Description = "Social - Other Violence", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Social - Other", Description = "Social - Other", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Social - Self Esteem / Anger Management", Description = "Social - Self Esteem / Anger Management", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Special Needs", Description = "Special Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Transportation", Description = "Transportation", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Tutoring - Other", Description = "Tutoring - Other", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Tutoring - Primary", Description = "Tutoring - Primary", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Tutoring - Junior High", Description = "Tutoring - Junior High", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Tutoring - Secondary", Description = "Tutoring - Secondary", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Tutoring - Post-Secondary", Description = "Tutoring - Post-Secondary", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Service { ServiceProviderID=1, Name = "Youth at Risk", Description = "Youth at Risk", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                
                return items;
            }
        }

        public static List<HousingQuality> HousingQuality
        {
            get
            {
                List<HousingQuality> items = new List<HousingQuality>();
                items.Add(new HousingQuality { Name = "Insufficient Space / Crowded", Description = "Insufficient Space / Crowded", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new HousingQuality { Name = "Few Basic Amenities (Inadequate Electrical Installation, Plumbing, Light)", Description = "Few Basic Amenities (Inadequate Electrical Installation, Plumbing, Light)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new HousingQuality { Name = "Unaffordable (Given Financial Means)", Description = "Unaffordable (Given Financial Means)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new HousingQuality { Name = "Unhealthy Neighborhood (Quiet, Unpolluted, Safe)", Description = "Unhealthy Neighborhood (Quiet, Unpolluted, Safe)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new HousingQuality { Name = "More than One (Specify in Notes)", Description = "More than One (Specify in Notes)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                return items;
            }
        }

        public static List<ServiceLevelOutcome> ServiceLevelOutcome
        {
            get
            {
                List<ServiceLevelOutcome> items = new List<ServiceLevelOutcome>();
                items.Add(new ServiceLevelOutcome { Name = "Aspiration Met", Description = "Aspiration Met", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ServiceLevelOutcome { Name = "Aspiration not Met", Description = "Aspiration not Met", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ServiceLevelOutcome { Name = "Deleted", Description = "Deleted", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ServiceLevelOutcome { Name = "Goal Met", Description = "Goal Met", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ServiceLevelOutcome { Name = "Goal Not Met", Description = "Goal Not Met", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ServiceLevelOutcome { Name = "In-Process", Description = "In-Process", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ServiceLevelOutcome { Name = "Not Started", Description = "Not Started", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ServiceLevelOutcome { Name = "On Hold", Description = "On Hold", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ServiceLevelOutcome { Name = "Stabilized", Description = "Stabilized", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ServiceLevelOutcome { Name = "Deterorated", Description = "Deterorated", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ServiceLevelOutcome { Name = "No Change", Description = "No Change", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                return items;
            }
        }

        public static List<ImmigrationCitizenshipStatus> ImmigrationCitizenshipStatus
        {
            get
            {
                List<ImmigrationCitizenshipStatus> items = new List<ImmigrationCitizenshipStatus>();
                items.Add(new ImmigrationCitizenshipStatus { Name = "Temporary Resident (Visitor, Student, Temporary Worker)", Description = "Temporary Resident (Visitor, Student, Temporary Worker)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ImmigrationCitizenshipStatus { Name = "Refugee Claimant", Description = "Refugee Claimant", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ImmigrationCitizenshipStatus { Name = "Permanent Resident (Landed Immigrant)", Description = "Permanent Resident (Landed Immigrant)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new ImmigrationCitizenshipStatus { Name = "Citizen", Description = "Citizen", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                return items;
            }
        }

        public static List<IndividualStatus> IndividualStatus
        {
            get
            {
                List<IndividualStatus> items = new List<IndividualStatus>();
                items.Add(new IndividualStatus { Name = "Active", Description = "Active", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new IndividualStatus { Name = "Not a Client", Description = "Not a Client", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new IndividualStatus { Name = "On Boarding", Description = "On Boarding", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new IndividualStatus { Name = "On Going Support and Monitoring", Description = "On Going Support and Monitoring", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new IndividualStatus { Name = "Referred and Monitored", Description = "Referred and Monitored", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new IndividualStatus { Name = "Closed - Completed", Description = "Closed - Completed", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new IndividualStatus { Name = "Closed - Deceased", Description = "Closed - Deceased", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new IndividualStatus { Name = "Closed - Not Qualified", Description = "Closed - Not Qualified", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new IndividualStatus { Name = "Closed - Relocated", Description = "Closed - Relocated", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new IndividualStatus { Name = "Closed - Unsuccesful Completion", Description = "Closed - Unsuccesful Completion", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new IndividualStatus { Name = "Closed - Lack of Family Engagement", Description = "Closed - Lack of Family Engagement", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new IndividualStatus { Name = "Closed - Family Withdrew", Description = "Closed - Family Withdrew", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                return items;
            }
        }

        public static List<AssessmentType> AssessmentType
        {
            get
            {
                List<AssessmentType> items = new List<AssessmentType>();
                items.Add(new AssessmentType { Name = "Initial", Description = "Initial", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new AssessmentType { Name = "Interim", Description = "Interim", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new AssessmentType { Name = "Discharge", Description = "Discharge", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                return items;
            }
        }

        public static List<ProfileType> ProfileType
        {
            get
            {
                List<ProfileType> items = new List<ProfileType>();
                items.Add(new ProfileType { Name = "Initial", Description = "Initial", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new ProfileType { Name = "Interim", Description = "Interim", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new ProfileType { Name = "Discharge", Description = "Discharge", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                return items;
            }
        }

        public static List<RiskType> RiskType
        {
            get
            {
                List<RiskType> items = new List<RiskType>();
                items.Add(new RiskType { Name = "High", Description = "High", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new RiskType { Name = "Medium", Description = "Medium", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new RiskType { Name = "Low", Description = "Low", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                return items;
            }
        }

        public static List<FinancialAssistanceCategory> FinancialAssistanceCategory
        {
            get
            {
                List<FinancialAssistanceCategory> items = new List<FinancialAssistanceCategory>();
                items.Add(new FinancialAssistanceCategory { Name = "BC", Description = "BC", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceCategory { Name = "EDM", Description = "EDM", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceCategory { Name = "PRS", Description = "PRS", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new FinancialAssistanceCategory { Name = "ONT", Description = "ONT", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new FinancialAssistanceCategory { Name = "OTT", Description = "OTT", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new FinancialAssistanceCategory { Name = "Q&M", Description = "Q&M", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                return items;
            }
        }

        public static List<FinancialAssistanceSubCategory> FinancialAssistanceSubCategory
        {
            get
            {
                List<FinancialAssistanceSubCategory> items = new List<FinancialAssistanceSubCategory>();

                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 1, Name = "Counseling and Medical Referral", Description = "Counseling and Medical Referral", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 1, Name = "Education / Tuition / Training", Description = "Education / Tuition / Training", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 1, Name = "Tutoring", Description = "Tutoring", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 1, Name = "Food / Groceries", Description = "Food / Groceries", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 1, Name = "Rent / Housing / Moving", Description = "Rent / Housing / Moving", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 1, Name = "Living Allowance / Home-related", Description = "Living Allowance / Home-related", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 1, Name = "Transportation", Description = "Transportation", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 1, Name = "Child Care", Description = "Child Care", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 1, Name = "Burial Expense Relief Plan", Description = "Burial Expense Relief Plan", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 1, Name = "Camps (Institutional)", Description = "Camps (Istitutional)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 1, Name = "Youth - Extracurricular Programs", Description = "Youth - Extracurricular Programs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 1, Name = "Special Needs (SKIP)", Description = "Special Needs (SKIP)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 2, Name = "Counseling and Medical Referral", Description = "Counseling and Medical Referral", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 2, Name = "Education / Tuition / Training", Description = "Education / Tuition / Training", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 2, Name = "Tutoring", Description = "Tutoring", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 2, Name = "Food / Groceries", Description = "Food / Groceries", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 2, Name = "Rent / Housing / Moving", Description = "Rent / Housing / Moving", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 2, Name = "Living Allowance / Home-related", Description = "Living Allowance / Home-related", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 2, Name = "Transportation", Description = "Transportation", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 2, Name = "Child Care", Description = "Child Care", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 2, Name = "Burial Expense Relief Plan", Description = "Burial Expense Relief Plan", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 2, Name = "Camps (Institutional)", Description = "Camps (Istitutional)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 2, Name = "Youth - Extracurricular Programs", Description = "Youth - Extracurricular Programs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 2, Name = "Special Needs (SKIP)", Description = "Special Needs (SKIP)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 3, Name = "Counseling and Medical Referral", Description = "Counseling and Medical Referral", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 3, Name = "Education / Tuition / Training", Description = "Education / Tuition / Training", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 3, Name = "Tutoring", Description = "Tutoring", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 3, Name = "Food / Groceries", Description = "Food / Groceries", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 3, Name = "Rent / Housing / Moving", Description = "Rent / Housing / Moving", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 3, Name = "Living Allowance / Home-related", Description = "Living Allowance / Home-related", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 3, Name = "Transportation", Description = "Transportation", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 3, Name = "Child Care", Description = "Child Care", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 3, Name = "Burial Expense Relief Plan", Description = "Burial Expense Relief Plan", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 3, Name = "Camps (Institutional)", Description = "Camps (Istitutional)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 3, Name = "Youth - Extracurricular Programs", Description = "Youth - Extracurricular Programs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 3, Name = "Special Needs (SKIP)", Description = "Special Needs (SKIP)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 4, Name = "Counseling and Medical Referral", Description = "Counseling and Medical Referral", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 4, Name = "Education / Tuition / Training", Description = "Education / Tuition / Training", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 4, Name = "Tutoring", Description = "Tutoring", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 4, Name = "Food / Groceries", Description = "Food / Groceries", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 4, Name = "Rent / Housing / Moving", Description = "Rent / Housing / Moving", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 4, Name = "Living Allowance / Home-related", Description = "Living Allowance / Home-related", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 4, Name = "Transportation", Description = "Transportation", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 4, Name = "Child Care", Description = "Child Care", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 4, Name = "Burial Expense Relief Plan", Description = "Burial Expense Relief Plan", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 4, Name = "Camps (Institutional)", Description = "Camps (Istitutional)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 4, Name = "Youth - Extracurricular Programs", Description = "Youth - Extracurricular Programs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 4, Name = "Special Needs (SKIP)", Description = "Special Needs (SKIP)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 5, Name = "Counseling and Medical Referral", Description = "Counseling and Medical Referral", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 5, Name = "Education / Tuition / Training", Description = "Education / Tuition / Training", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 5, Name = "Tutoring", Description = "Tutoring", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 5, Name = "Food / Groceries", Description = "Food / Groceries", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 5, Name = "Rent / Housing / Moving", Description = "Rent / Housing / Moving", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 5, Name = "Living Allowance / Home-related", Description = "Living Allowance / Home-related", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 5, Name = "Transportation", Description = "Transportation", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 5, Name = "Child Care", Description = "Child Care", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 5, Name = "Burial Expense Relief Plan", Description = "Burial Expense Relief Plan", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 5, Name = "Camps (Institutional)", Description = "Camps (Istitutional)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 5, Name = "Youth - Extracurricular Programs", Description = "Youth - Extracurricular Programs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 5, Name = "Special Needs (SKIP)", Description = "Special Needs (SKIP)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 6, Name = "Counseling and Medical Referral", Description = "Counseling and Medical Referral", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 6, Name = "Education / Tuition / Training", Description = "Education / Tuition / Training", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 6, Name = "Tutoring", Description = "Tutoring", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 6, Name = "Food / Groceries", Description = "Food / Groceries", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 6, Name = "Rent / Housing / Moving", Description = "Rent / Housing / Moving", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 6, Name = "Living Allowance / Home-related", Description = "Living Allowance / Home-related", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 6, Name = "Transportation", Description = "Transportation", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 6, Name = "Child Care", Description = "Child Care", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 6, Name = "Burial Expense Relief Plan", Description = "Burial Expense Relief Plan", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 6, Name = "Camps (Institutional)", Description = "Camps (Istitutional)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 6, Name = "Youth - Extracurricular Programs", Description = "Youth - Extracurricular Programs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new FinancialAssistanceSubCategory { FinancialAssistanceCategoryID = 6, Name = "Special Needs (SKIP)", Description = "Special Needs (SKIP)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                
                return items;
            }
        }

        public static List<ReasonsForDischarge> ReasonsForDischarge
        {
            get
            {
                List<ReasonsForDischarge> items = new List<ReasonsForDischarge>();
                items.Add(new ReasonsForDischarge { Name = "Declined Services", Description = "Declined Services", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new ReasonsForDischarge { Name = "Death", Description = "Death", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new ReasonsForDischarge { Name = "Goals Met", Description = "Goals Met", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new ReasonsForDischarge { Name = "Lost To Contact", Description = "Lost To Contact", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new ReasonsForDischarge { Name = "Moved Out Of Area", Description = "Moved Out Of Area", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new ReasonsForDischarge { Name = "No further services wanted/required", Description = "No further services wanted/required", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new ReasonsForDischarge { Name = "Open Current referral", Description = "Open Current referral", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new ReasonsForDischarge { Name = "Other", Description = "Other", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new ReasonsForDischarge { Name = "Referred to community outreach for services – specify", Description = "Referred to community outreach for services – specify", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new ReasonsForDischarge { Name = "Referred to internal institutions for services", Description = "Referred to internal institutions for services", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new ReasonsForDischarge { Name = "Unable to engage", Description = "Unable to engage", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new ReasonsForDischarge { Name = "Inquiry only", Description = "Inquiry only", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                items.Add(new ReasonsForDischarge { Name = "Immediate goals met & Ongoing monitoring", Description = "Immediate goals met & Ongoing monitoring", IsActive = true, CreateDate=DateTime.Now, LastUpdateDate=DateTime.Now });
                return items;
            }
        }

        public static List<QualityOfLifeCategory> QualityOfLifeCategory
        {
            get
            {
                List<QualityOfLifeCategory> items = new List<QualityOfLifeCategory>();
                items.Add(new QualityOfLifeCategory { Name = "Assets and Production", Description = "Assets and Production", IsActive = false, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeCategory { Name = "Dignity and Self Respect", Description = "Dignity and Self Respect", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeCategory { Name = "Education", Description = "Education", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeCategory { Name = "Health", Description = "Health", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeCategory { Name = "Housing", Description = "Housing", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeCategory { Name = "Immediate Needs", Description = "Immediate Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeCategory { Name = "Income and Livelihood", Description = "Income and Livelihood", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeCategory { Name = "Social Support", Description = "Social Support", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                return items;
            }
        }
                
        public static List<QualityOfLifeSubCategory> QualityOfLifeSubCategory
        {
            get
            {
                List<QualityOfLifeSubCategory> items = new List<QualityOfLifeSubCategory>();
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 1, Name = "Family Savings", Description = "Family Savings", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 1, Name = "Financial Literacy", Description = "Financial Literacy", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 1, Name = "Employability", Description = "Employability", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 2, Name = "General Emotional Wellbeing", Description = "General Emotional Wellbeing", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 2, Name = "Hope", Description = "Hope", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 2, Name = "Empowerment", Description = "Empowerment", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 3, Name = "Individual", Description = "Individual", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 3, Name = "Family", Description = "Family", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 3, Name = "Access To", Description = "Access To", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 3, Name = "Language Needs", Description = "Language Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 4, Name = "Mental Health / Emotional Well Being", Description = "Mental Health / Emotional Well Being", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 4, Name = "Disability", Description = "Disability", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 4, Name = "Mental Illness", Description = "Mental Illness", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 4, Name = "Physical Health", Description = "Physical Health", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 4, Name = "General", Description = "General", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });


                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 5, Name = "General", Description = "General", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 5, Name = "Internal Enviornment", Description = "Internal Enviornment", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 5, Name = "External Enviornment", Description = "External Enviornment", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 6, Name = "General", Description = "General", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 7, Name = "Employment", Description = "Employment", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 7, Name = "Financial Security", Description = "Financial Security", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 8, Name = "Social & Cultural", Description = "Social & Cultural", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 8, Name = "Faith Considerations", Description = "Faith Considerations", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLifeSubCategory { QualityOfLifeCategoryID = 8, Name = "Legal Considerations", Description = "Legal Considerations", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                
                return items;
            }
        }

        public static List<QualityOfLife> QualityOfLife
        {
            get
            {
                List<QualityOfLife> items = new List<QualityOfLife>();

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 1, Name = "Number of Family Members that Save", Description = "Number of Family Members that Save", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 1, Name = "Increase in Family Savings", Description = "Increase in Family Savings", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 2, Name = "Debt Management", Description = "Debt Management", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 2, Name = "Budgeting Skills", Description = "Budgeting Skills", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 2, Name = "Knowledge of Credit and Banking Systems", Description = "Knowledge of Credit and Banking Systems", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 3, Name = "Job Specific Skills / Training", Description = "Job Specific Skills / Training", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 3, Name = "Job Readiness Skills (e.g. Resume, Interviews)", Description = "Job Readiness Skills (e.g. Resume, Interviews)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 3, Name = "Basic Computer Skills", Description = "Basic Computer Skills", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 3, Name = "Job Search Skills", Description = "Job Search Skills", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 4, Name = "Self Esteem / Confidence", Description = "Self Esteem / Confidence", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 4, Name = "Motivation", Description = "Motivation", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 4, Name = "Resillience", Description = "Resillience", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 5, Name = "Feelings of Hopelessness", Description = "Feelings of Hopelessness", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 6, Name = "Agency to Influence Own Life", Description = "Agency to Influence Own Life", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 6, Name = "Self Respect", Description = "Self Respect", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 6, Name = "Sexuality (Lesbian Gay Bisexual Transgender & Intersex)", Description = "Sexuality (Lesbian Gay Bisexual Transgender & Intersex)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 7, Name = "Level Of Education", Description = "Level Of Education", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 7, Name = "Additional Training / Courses", Description = "Additional Training / Courses", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 7, Name = "Tutorial Assistance", Description = "Tutorial Assistance", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 7, Name = "Career Counseling", Description = "Career Counseling", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 7, Name = "Graduation / Program Completion", Description = "Graduation / Program Completion", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 7, Name = "Program Admission", Description = "Program Admission", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 8, Name = "Parental Involvement", Description = "Parental Involvement", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 8, Name = "Allocate Resources to Support Education Needs", Description = "Allocate Resources to Support Education Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 9, Name = "Internal and External Funding", Description = "Internal and External Funding", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 9, Name = "Tutorial Services", Description = "Tutorial Services", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 9, Name = "Summer Camps", Description = "Summer Camps", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 9, Name = "Early Childhood Programs / Child Care", Description = "Early Childhood Programs / Child Care", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 9, Name = "Better Schools", Description = "Better Schools", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 9, Name = "Special Needs Support", Description = "Special Needs Support", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 10, Name = "English or French", Description = "English or French", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 10, Name = "Other Language", Description = "Other Language", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 10, Name = "Language Enhancements", Description = "Language Enhancements", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 11, Name = "Bereavement Loss", Description = "Bereavement Loss", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 11, Name = "Isolation ", Description = "Isolation ", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 11, Name = "Loneliness", Description = "Loneliness", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 11, Name = "Bullying (including cyber bullying)", Description = "Bullying (including cyber bullying)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 11, Name = "Stress / Anxiety", Description = "Stress / Anxiety", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 11, Name = "Psycho-Emotional Issues", Description = "Psycho-Emotional Issues", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 11, Name = "Non-Physical Forms of Abuse (e.g. verbal, emotional)", Description = "Non-Physical Forms of Abuse (e.g. verbal, emotional)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 11, Name = "Interpersonal Conflict", Description = "Interpersonal Conflict", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 12, Name = "Intellectual", Description = "Intellectual", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 12, Name = "Sensory", Description = "Sensory", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 12, Name = "Physical", Description = "Physical", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 12, Name = "Multiple", Description = "Multiple", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 12, Name = "Developmental / Learning", Description = "Developmental / Learning", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 12, Name = "Assistive Devices", Description = "Assistive Devices", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 13, Name = "Dementia / Cognitive Decline", Description = "Dementia / Cognitive Decline", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 13, Name = "Bipolar Disorder", Description = "Bipolar Disorder", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 13, Name = "Psychosis", Description = "Psychosis", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 13, Name = "Depression", Description = "Depression", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 13, Name = "Behavioural Disorder (e.g. Obsessive-Compulsive Disorder, Attention Deficit Hyperactivity Disorder, Oppositional Defiant Disorder)", Description = "Behavioural Disorder (e.g. Obsessive-Compulsive Disorder, Attention Deficit Hyperactivity Disorder, Oppositional Defiant Disorder)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 13, Name = "Posttraumatic Stress Disorder", Description = "Posttraumatic Stress Disorder", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 13, Name = "Autism Spectrum", Description = "Autism Spectrum", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 13, Name = "Personality Disorder", Description = "Personality Disorder", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 14, Name = "Physical Health", Description = "Physical Health", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 14, Name = "Chronic Disease", Description = "Chronic Disease", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 14, Name = "Injury", Description = "Injury", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 14, Name = "Self Care (e.g. hygiene, dressing, medication and meal intake etc)", Description = "Self Care (e.g. hygiene, dressing, medication and meal intake etc)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 14, Name = "Sexual and Reproductive Health", Description = "Sexual and Reproductive Health", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 14, Name = "Other", Description = "Other", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 15, Name = "Family Physician Access", Description = "Family Physician Access", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 15, Name = "Dental Services Access", Description = "Dental Services Access", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 15, Name = "Navigation of Health System", Description = "Navigation of Health System", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 16, Name = "Availability of Housing", Description = "Availability of Housing", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 16, Name = "Affordability of Housing", Description = "Affordability of Housing", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 16, Name = "Landlord / Tenant Dispute", Description = "Landlord / Tenant Dispute", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 16, Name = "Foreclosure / Threat of Foreclosure", Description = "Foreclosure / Threat of Foreclosure", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 16, Name = "Accessibility of Long-Term Care", Description = "Accessibility of Long-Term Care", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 17, Name = "Overcrowding ", Description = "Overcrowding ", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 17, Name = "Poor Amenities", Description = "Poor Amenities", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 17, Name = "Furniture / Belongings", Description = "Furniture / Belongings", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 17, Name = "Environmental Hazards", Description = "Environmental Hazards", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 17, Name = "Cleanliness / Maintenance", Description = "Cleanliness / Maintenance", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 17, Name = "Adaptive Devices", Description = "Adaptive Devices", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 17, Name = "Unsafe", Description = "Unsafe", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 18, Name = "Amenities and Services", Description = "Amenities and Services", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 18, Name = "Transportation Access", Description = "Transportation Access", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 18, Name = "Neighbourhood Hazards", Description = "Neighbourhood Hazards", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 18, Name = "Accessibility", Description = "Accessibility", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Accommodation", Description = "Accommodation", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Medical", Description = "Medical", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Physical Abuse", Description = "Physical Abuse", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Sexual Abuse", Description = "Sexual Abuse", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Non-Physical Forms of Abuse (e.g. verbal, emotional)", Description = "Non-Physical Forms of Abuse (e.g. verbal, emotional)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Child Abuse", Description = "Child Abuse", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Physical Safety", Description = "Physical Safety", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Legal", Description = "Legal", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Immigration/Deportation", Description = "Immigration/Deportation", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Clothing", Description = "Clothing", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Financial Hardship", Description = "Financial Hardship", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Addiction", Description = "Addiction", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Food", Description = "Food", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Neglect", Description = "Neglect", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Suicide Intervention", Description = "Suicide Intervention", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Other", Description = "Other", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 19, Name = "Mental Health Crisis", Description = "Mental Health Crisis", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 20, Name = "Unemployment", Description = "Unemployment", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 20, Name = "Underemployment", Description = "Underemployment", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 20, Name = "Professional Advancement /Entrepreneurship", Description = "Professional Advancement /Entrepreneurship", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 21, Name = "Family Income under Low Income Cut-Off (LICO)", Description = "Family Income under Low Income Cut-Off (LICO)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 21, Name = "Family Income under $20,000 (Ultra-Poor)", Description = "Family Income under $20,000 (Ultra-Poor)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 21, Name = "Changes to Family Income", Description = "Changes to Family Income", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 21, Name = "Access to Government Income Programs", Description = "Access to Government Income Programs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 22, Name = "Family", Description = "Family", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 22, Name = "Internal or External Community", Description = "Internal or External Community", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 22, Name = "Friends", Description = "Friends", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 22, Name = "Involvement with Institutions", Description = "Involvement with Institutions", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 22, Name = "Relationships and Dynamics - Marital", Description = "Relationships and Dynamics - Marital", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 22, Name = "Relationships and Dynamics - Child", Description = "Relationships and Dynamics - Child", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 22, Name = "Relationships and Dynamics - Extended Family", Description = "Relationships and Dynamics - Extended Family", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 22, Name = "Relationships and Dynamics -Other", Description = "Relationships and Dynamics -Other", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 22, Name = "Transportation", Description = "Transportation", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 22, Name = "Support Network", Description = "Support Network", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 22, Name = "Coping and Problem Solving Skills", Description = "Coping and Problem Solving Skills", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 22, Name = "Parenting Skills", Description = "Parenting Skills", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 22, Name = "Hobbies and Interests / Leisure Activities", Description = "Hobbies and Interests / Leisure Activities", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 22, Name = "Communication", Description = "Communication", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 23, Name = "Identity", Description = "Identity", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 23, Name = "Sense of Belonging", Description = "Sense of Belonging", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 23, Name = "Participation in Faith Based Activities", Description = "Participation in Faith Based Activities", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 23, Name = "Prayers and Jamatkhana Attendance", Description = "Prayers and Jamatkhana Attendance", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 23, Name = "Boating Under the Influence Attendance", Description = "Boating Under the Influence Attendance", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 23, Name = "Interfaith Family Considerations", Description = "Interfaith Family Considerations", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 24, Name = "Guardianship", Description = "Guardianship", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 24, Name = "Criminal", Description = "Criminal", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 24, Name = "Family", Description = "Family", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 24, Name = "Child Support", Description = "Child Support", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 24, Name = "Business", Description = "Business", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 24, Name = "Wills and Estates / Power of Attorney", Description = "Wills and Estates / Power of Attorney", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 24, Name = "Other", Description = "Other", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 24, Name = "Spousal", Description = "Spousal", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new QualityOfLife { QualityOfLifeSubCategoryID = 24, Name = "Immigration", Description = "Immigration", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                

                return items;
            }
        }

        public static List<RelationshipStatus> RelationshipStatus
        {
            get
            {
                List<RelationshipStatus> items = new List<RelationshipStatus>();
                items.Add(new RelationshipStatus { Name = "Aunt", Description = "Aunt", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Brother", Description = "Brother", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Brother-in-law", Description = "Brother-in-law", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Family Contact", Description = "Family Contact", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Daughter", Description = "Daughter", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Daughter-in-law", Description = "Daughter-in-law", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Father", Description = "Father", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Father-in-law", Description = "Father-in-law", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Granddaughter", Description = "Granddaughter", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Grandson", Description = "Grandson", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Grandmother", Description = "Grandmother", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Grandfather", Description = "Grandfather", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Spouce", Description = "Spouce", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Mother", Description = "Mother", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Mother-in-law", Description = "Mother-in-law", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Other", Description = "Other", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Sister", Description = "Sister", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Sister-in-law", Description = "Sister-in-law", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Son", Description = "Son", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Son-in-law", Description = "Son-in-law", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Uncle", Description = "Uncle", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Stepson", Description = "Stepson", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Stepdaughter", Description = "Stepdaughter", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Ex-Wife", Description = "Ex-Wife", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Ex-Husband", Description = "Ex-Husband", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Stepfather", Description = "Stepfather", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "StepMother", Description = "StepMother", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Niece", Description = "Niece", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Nephew", Description = "Nephew", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new RelationshipStatus { Name = "Cousin", Description = "Cousin", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                return items;
            }
        }

        public static List<TimeSpent> TimeSpent
        {
            get
            {
                List<TimeSpent> items = new List<TimeSpent>();
                items.Add(new TimeSpent { Name = "10 min", Description = "10 min", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "20 min", Description = "20 min", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "30 min", Description = "30 min", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "45 min", Description = "45 min", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "60 min", Description = "60 min", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "75 min", Description = "75 min", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "90 min", Description = "90 min", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "120 min", Description = "120 min", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "2.5 hrs", Description = "2.5 hrs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "3 hrs", Description = "3 hrs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "4 hrs", Description = "4 hrs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "5 hrs", Description = "5 hrs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "6 hrs", Description = "6 hrs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "7 hrs", Description = "7 hrs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "8 hrs", Description = "8 hrs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "9 hrs", Description = "9 hrs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "10 hrs", Description = "10 hrs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "12 hrs", Description = "12 hrs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new TimeSpent { Name = "15 hrs", Description = "15 hrs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                
                return items;
            }
        }

        public static List<SmartGoal> SmartGoal
        {
            get
            {
                List<SmartGoal> items = new List<SmartGoal>();
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 1, Name = "Increase Savings", Description = "Increase Savings", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 1, Name = "Acquire Ownership of an Asset (e.g. Car, Home, Equipment)", Description = "Acquire Ownership of an Asset (e.g. Car, Home, Equipment)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 1, Name = "Increase Awareness of Specific Services Available within the Jamat to Address Identified Needs", Description = "Increase Awareness of Specific Services Available within the Jamat to Address Identified Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 1, Name = "Increase Awareness of Specific Services Available in the External Community to Address Identified Needs", Description = "Increase Awareness of Specific Services Available in the External Community to Address Identified Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 1, Name = "Increase Financial Literacy Skills Related to [Choose One]:  Credit; Assets; Consumerism; Budgeting; and/or Banking", Description = "Increase Financial Literacy Skills Related to [Choose One]:  Credit; Assets; Consumerism; Budgeting; and/or Banking", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 1, Name = "Other", Description = "Other", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new SmartGoal { QualityOfLifeCategoryID = 2, Name = "Increase Self-Confidence or Self Esteem", Description = "Increase Self-Confidence or Self Esteem", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 2, Name = "Improve Coping Skills or Sense of Resiliency", Description = "Improve Coping Skills or Sense of Resiliency", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 2, Name = "Feel more Respected by Family Members", Description = "Feel more Respected by Family Members", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 2, Name = "Feel more Respected by Faith Community Members", Description = "Feel more Respected by Faith Community Members", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 2, Name = "Feel more Respected in the Neighbourhood or Work Community", Description = "Feel more Respected in the Neighbourhood or Work Community", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 2, Name = "Feel more Hopeful", Description = "Feel more Hopeful", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 2, Name = "Increase Trust and Respect for and from Jamati Leadership", Description = "Increase Trust and Respect for and from Jamati Leadership", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 2, Name = "Increase Confidence and Trust in Jamati Services or Programs for Meeting Immediate Needs", Description = "Increase Confidence and Trust in Jamati Services or Programs for Meeting Immediate Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 2, Name = "Increase Confidence and Trust in Community Outreach Services or Agencies for Meeting Immediate Needs", Description = "Increase Confidence and Trust in Community Outreach Services or Agencies for Meeting Immediate Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 2, Name = "Increase Awareness of Specific Services Available within the Jamat to Address Identified Needs", Description = "Increase Awareness of Specific Services Available within the Jamat to Address Identified Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 2, Name = "Increase Awareness of Specific Services Available in the External Community to Address Identified Needs", Description = "Increase Awareness of Specific Services Available in the External Community to Address Identified Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 2, Name = "Change Personal/Family Routine to Improve Social or Emotional Well Being", Description = "Change Personal/Family Routine to Improve Social or Emotional Well Being", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 2, Name = "Other", Description = "Other", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new SmartGoal { QualityOfLifeCategoryID = 3, Name = "Upgrade Existing Technical Skills", Description = "Upgrade Existing Technical Skills", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 3, Name = "Acquire new Technical Skills", Description = "Acquire new Technical Skills", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 3, Name = "Improve Grades by [Choose One]:  One Letter Grade; Two Letter Grades; Three or more Letter Grades", Description = "Improve Grades by [Choose One]:  One Letter Grade; Two Letter Grades; Three or more Letter Grades", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 3, Name = "Improve Communication Skills", Description = "Improve Communication Skills", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 3, Name = "Improve Language Skills/Literacy [Choose One]:  English or French, Home Language", Description = "Improve Language Skills/Literacy [Choose One]:  English or French, Home Language", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 3, Name = "Improve Leadership Skills", Description = "Improve Leadership Skills", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 3, Name = "Increase Awareness of Services Available within the Jamat/Institutions to Address Identified Needs", Description = "Increase Awareness of Services Available within the Jamat/Institutions to Address Identified Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 3, Name = "Increase Awareness of Services Available within the External Community to Address Identified Needs", Description = "Increase Awareness of Services Available within the External Community to Address Identified Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 3, Name = "Enroll in a New or Continuing/Higher-Level Educational Program", Description = "Enroll in a New or Continuing/Higher-Level Educational Program", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 3, Name = "Acquire Resources to Enable New or Further Education", Description = "Acquire Resources to Enable New or Further Education", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 3, Name = "Other", Description = "Other", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });


                items.Add(new SmartGoal { QualityOfLifeCategoryID = 4, Name = "Acquire or Increase Access to Professional Care for an Existing or Suspected Physical Health Condition", Description = "Acquire or Increase Access to Professional Care for an Existing or Suspected Physical Health Condition", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 4, Name = "Acquire or Increase Access to Professional Care for an Existing or Suspected Mental Health Condition", Description = "Acquire or Increase Access to Professional Care for an Existing or Suspected Mental Health Condition", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 4, Name = "Acquire or Increase Access to Professional Care for Other Health Assessments or Inquiries", Description = "Acquire or Increase Access to Professional Care for Other Health Assessments or Inquiries", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 4, Name = "Increase Awareness of Specific Services Available within the Jamat to Address Identified Needs", Description = "Increase Awareness of Specific Services Available within the Jamat to Address Identified Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 4, Name = "Increase Awareness of Specific Services Available in the External Community to Address Identified Needs", Description = "Increase Awareness of Specific Services Available in the External Community to Address Identified Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 4, Name = "Change Personal/Family Routine to Improve Mental Health Status (No Need for Medical or Social Support)", Description = "Change Personal/Family Routine to Improve Mental Health Status (No Need for Medical or Social Support)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 4, Name = "Change Personal/Family Routine to Improve Physical Health Status (No Need for Medical or Social Support)", Description = "Change Personal/Family Routine to Improve Physical Health Status (No Need for Medical or Social Support)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 4, Name = "Other", Description = "Other", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new SmartGoal { QualityOfLifeCategoryID = 5, Name = "Relocated to Temporary, Safe Housing Immediately", Description = "Relocated to Temporary, Safe Housing Immediately", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 5, Name = "Improve Current Housing Quality or Conditions", Description = "Improve Current Housing Quality or Conditions", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 5, Name = "Relocated to Improve Housing Quality or Conditions", Description = "Relocated to Improve Housing Quality or Conditions", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 5, Name = "Improving Basic Amenities [Choose One]: Adequate Electrical Installation; Plumbing; Light)", Description = "Improving Basic Amenities [Choose One]: Adequate Electrical Installation; Plumbing; Light)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 5, Name = "Relocate to More Affordable Housing", Description = "Relocate to More Affordable Housing", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 5, Name = "Relocate to New Neighbourhood to Enable Access to Services", Description = "Relocate to New Neighbourhood to Enable Access to Services", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 5, Name = "Relocate to Safer Neighbourhood", Description = "Relocate to Safer Neighbourhood", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 5, Name = "Improve Neighbourhood Health (More Quiet, Unpolluted, Safe)", Description = "Improve Neighbourhood Health (More Quiet, Unpolluted, Safe)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 5, Name = "Upgrade Housing Status (e.g. From Subsidized to Rental or Rental to Ownership)", Description = "Upgrade Housing Status (e.g. From Subsidized to Rental or Rental to Ownership)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 5, Name = "Increase Awareness of Specific Services Available Within the Jamat to Address Identified Needs", Description = "Increase Awareness of Specific Services Available Within the Jamat to Address Identified Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 5, Name = "Increase Awareness of Specific Services Available in the External Community to Address Identified Needs", Description = "Increase Awareness of Specific Services Available in the External Community to Address Identified Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 5, Name = "Other", Description = "Other", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new SmartGoal { QualityOfLifeCategoryID = 7, Name = "Increase Income", Description = "Increase Income", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 7, Name = "Obtain Employment", Description = "Obtain Employment", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 7, Name = "Improve Employment Status", Description = "Improve Employment Status", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 7, Name = "Improve Credit Rating", Description = "Improve Credit Rating", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 7, Name = "Gain or Increase Access to Credit", Description = "Gain or Increase Access to Credit", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 7, Name = "Greater Access to Government Income Security Programs / Social Benefits", Description = "Greater Access to Government Income Security Programs / Social Benefits", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 7, Name = "Improve Personal / Family Budgeting", Description = "Improve Personal / Family Budgeting", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 7, Name = "Reduce Debt", Description = "Reduce Debt", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 7, Name = "Increase Financial Literacy Skills Related to [Choose One]:  Credit; Assets; Consumerism; Budgeting; and/or Banking", Description = "Increase Financial Literacy Skills Related to [Choose One]:  Credit; Assets; Consumerism; Budgeting; and/or Banking", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 7, Name = "Increase Awareness of Specific Services Available within the Jamat to Address Identified Needs", Description = "Increase Awareness of Specific Services Available within the Jamat to Address Identified Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 7, Name = "Increase Awareness of Specific Services Available in the External Community to Address Identified Needs", Description = "Increase Awareness of Specific Services Available in the External Community to Address Identified Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 7, Name = "Other", Description = "Other", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Participation", Description = "Participation", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Increase Participation in the Non-Ismaili Community", Description = "Increase Participation in the Non-Ismaili Community", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Increase Use of Family and Friendship Networks to Support Needs and Goals", Description = "Increase Use of Family and Friendship Networks to Support Needs and Goals", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Increase Access to Respite Care", Description = "Increase Access to Respite Care", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Increase Mental Health and Wellness Support within Family", Description = "Increase Mental Health and Wellness Support within Family", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Increase Access to Mental Health and Wellness Supports within the Community", Description = "Increase Access to Mental Health and Wellness Supports within the Community", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Increase Attendance in Jamatkhana", Description = "Increase Attendance in Jamatkhana", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Increase Participation in Jamatkhana / Functions", Description = "Increase Participation in Jamatkhana / Functions", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Religious Experience", Description = "Religious Experience", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Gain More Knowledge / Education about Religion", Description = "Gain More Knowledge / Education about Religion", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Feel More Strongly Connected to the Faith", Description = "Feel More Strongly Connected to the Faith", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Increase Personal Solace from Religious Activities", Description = "Increase Personal Solace from Religious Activities", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Social Experience", Description = "Social Experience", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Increase Social Networks or Friendships", Description = "Increase Social Networks or Friendships", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Increase Sense of Connection to Faith Community", Description = "Increase Sense of Connection to Faith Community", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Increased Attendance in Religious Education", Description = "Increased Attendance in Religious Education", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Attitude Helping Others", Description = "Attitude Helping Others", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Increase Awareness of Specific Services Available within the Jamat to Address Identified Needs", Description = "Increase Awareness of Specific Services Available within the Jamat to Address Identified Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Increase Awareness of Specific Services Available in the External Community to Address Identified Needs", Description = "Increase Awareness of Specific Services Available in the External Community to Address Identified Needs", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new SmartGoal { QualityOfLifeCategoryID = 8, Name = "Other", Description = "Ohter", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                return items;
            }
        }
        
        public static List<Jamatkhana> Jamatkhana
        {
            get
            {
                List<Jamatkhana> items = new List<Jamatkhana>();
                items.Add(new Jamatkhana { RegionID = 1, Name = "Abbotsford", Description = "Abbotsford", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 1, Name = "Burnaby Lake", Description = "Burnaby Lake", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 1, Name = "Chilliwak", Description = "Chilliwak", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 1, Name = "Darkhana", Description = "Darkhana", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 1, Name = "Downtown", Description = "Downtown", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 1, Name = "Fraser Valley", Description = "Fraser Valley", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 1, Name = "Vancouver HQ (LionsGate)", Description = "Vancouver HQ (LionsGate)", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 1, Name = "Nanaimo", Description = "Nanaimo", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 1, Name = "Richmond", Description = "Richmond", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 1, Name = "Tri-City", Description = "Tri-City", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 1, Name = "Victoria", Description = "Victoria", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 1, Name = "Kelowna", Description = "Kelowna", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new Jamatkhana { RegionID = 2, Name = "Belle Rive", Description = "Belle Rive", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 2, Name = "Edmonton HQ", Description = "Edmonton HQ", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 2, Name = "Edmonton West", Description = "Edmonton West", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 2, Name = "Fort McMurray", Description = "Fort McMurray", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 2, Name = "Red Deer", Description = "Red Deer", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                
                items.Add(new Jamatkhana { RegionID = 4, Name = "Barrie", Description = "Barrie", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Bayview", Description = "Bayview", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Brampton", Description = "Brampton", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Brantford", Description = "Brantford", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Don Mills", Description = "Don Mills", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Dundas West", Description = "Dundas West", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Etobicoke", Description = "Etobicoke", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Guelph", Description = "Guelph", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Halton", Description = "Halton", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Hamilton", Description = "Hamilton", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Kitchener", Description = "Kitchener", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "London", Description = "London", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Mississauga", Description = "Mississauga", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Niagara Falls", Description = "Niagara Falls", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Oshawa", Description = "Oshawa", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Bobcaygeon", Description = "Bobcaygeon", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Pickering", Description = "Pickering", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Richmond Hill", Description = "Richmond Hill", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Scarborough", Description = "Scarborough", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Sudbury", Description = "Sudbury", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Tilsonberg", Description = "Tilsonberg", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "East York", Description = "East York", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Unionville", Description = "Unionville", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Willowdale", Description = "Willowdale", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Toronto HQ", Description = "Toronto HQ", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Meadowvale", Description = "Meadowvale", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Windsor", Description = "Windsor", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "Belleville", Description = "Belleville", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 4, Name = "St. Thomas", Description = "St. Thomas", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new Jamatkhana { RegionID = 5, Name = "Kingston", Description = "Kingston", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 5, Name = "Ottawa HQ", Description = "Ottawa HQ", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new Jamatkhana { RegionID = 6, Name = "Calgary Northeast", Description = "Calgary Northeast", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 6, Name = "Calgary HQ", Description = "Calgary HQ", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 6, Name = "Franklin", Description = "Franklin", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 6, Name = "Lethbridge", Description = "Lethbridge", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 6, Name = "Calgary Northwest ", Description = "Calgary Northwest ", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 6, Name = "Regina", Description = "Regina", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 6, Name = "Saskatoon", Description = "Saskatoon", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 6, Name = "Calgary South", Description = "Calgary South", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 6, Name = "Winnipeg", Description = "Winnipeg", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 6, Name = "Waterton", Description = "Waterton", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                items.Add(new Jamatkhana { RegionID = 7, Name = "Brossard", Description = "Brossard", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 7, Name = "Granby", Description = "Granby", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 7, Name = "Halifax", Description = "Halifax", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 7, Name = "Laval", Description = "Laval", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 7, Name = "Montreal HQ", Description = "Montreal HQ", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 7, Name = "Quebec City", Description = "Quebec City", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 7, Name = "Sherbrooke", Description = "Sherbrooke", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                items.Add(new Jamatkhana { RegionID = 7, Name = "St. John's", Description = "St. John's", IsActive = true, CreateDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                                
                return items;
            }
        }
    }
}


























