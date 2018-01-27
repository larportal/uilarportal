using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarpPortal.Classes
{
    public class cPhoneTest
    {
        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                if (_phoneNumber.Length < 3)
                {
                    _areaCode = _phoneNumber.Substring(0, _phoneNumber.Length);
                }
                else
                {
                    _areaCode = _phoneNumber.Substring(0, 3);
                }
                _originalPhoneNumber = value;
                if (_areaCode == "203") 
                {
                    _state = "CT"; 
                } 
                else 
                {
                    _state = "XX";
                }
                
            }
        }
        private string _areaCode;
        private string _originalPhoneNumber;
        private string _state;
        /// <summary>
        /// State is either CT or XX
        /// </summary>
        public string State { get { return _state; } }
        /// <summary>
        /// SomeName does some thing
        /// </summary>
        /// <param name="Param1">Gimme a state</param>
        public void SomeName(string Param1)
        {
        }


    }
}