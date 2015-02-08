/* =============================================================================
 * File:   KeySelectionViewModel.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2014 Cole Johnson
 * 
 * This file is part of iDecryptIt
 * 
 * iDecryptIt is free software: you can redistribute it and/or modify it under
 *   the terms of the GNU General Public License as published by the Free
 *   Software Foundation, either version 3 of the License, or (at your option)
 *   any later version.
 * 
 * iDecryptIt is distributed in the hope that it will be useful, but WITHOUT
 *   ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 *   FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
 *   more details.
 * 
 * You should have received a copy of the GNU General Public License along with
 *   iDecryptIt. If not, see <http://www.gnu.org/licenses/>.
 * =============================================================================
 */
using System.ComponentModel;

namespace Hexware.Programs.iDecryptIt
{
    public partial class KeySelectionViewModel
    {
        private string _id;
        private string _value;

        public KeySelectionViewModel()
        {
        }

        public string ID
        {
            get
            { return _id; }
            set
            {
                if (_id != value) {
                    _id = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }

        public string Value
        {
            get
            { return _value; }
            set
            {
                if (_value != value) {
                    _value = value;
                    NotifyPropertyChanged("Value");
                }
            }
        }
    }

    public partial class KeySelectionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}