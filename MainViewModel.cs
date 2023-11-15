using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;

namespace Data_Input_Validation
{
    public class MainViewModel : INotifyDataErrorInfo
    {
        Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

        public bool HasErrors => Errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (Errors.ContainsKey(propertyName))
            {
                return Errors[propertyName];

            }
            else
            {
               return Enumerable.Empty<string>();
            }

        }




        public void Validate(string propertyName, object propertyValue)
        {
            var results = new List<ValidationResult>();

            Validator.TryValidateProperty(propertyValue, new ValidationContext(this) { MemberName = propertyName }, results);


            if (results.Any())
            {
                Errors.Add(propertyName, results.Select(r => r.ErrorMessage).ToList());
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
            else
            {
                Errors.Remove(propertyName);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }



            SubmitCommand.RaiseCanExecuteChanged();

        }



        private string _name;

        [Required(ErrorMessage ="Name is Required")]
        public string Name
        {
            get { return _name; }
            set { _name = value;

                Validate(nameof(Name), value);            
            }
        }




        private string _email;

        [Required(ErrorMessage = "Email is Required")]
        public string Email
        {
            get { return _email; }
            set { _email = value;
                Validate(nameof(Email), value);

            }
        }



        private string _password;

        [Required(ErrorMessage = "Password is Required")]
        public string Password
        {
            get { return _password; }
            set { _password = value;
                Validate(nameof(Password), value);
            }
        }






        public ActionCommand SubmitCommand { get; set; }

        public MainViewModel()
        {


            SubmitCommand = new ActionCommand(Submit, CanSubmit);

        }

        private bool CanSubmit(object obj)
        {
            return Validator.TryValidateObject(this, new ValidationContext(this), null);

        }

        private void Submit(object obj)
        {
            MessageBox.Show("Submitted");
        }
    }
}
