using System;

using UIKit;

using Foundation;
using System.Collections.Generic;

namespace xamarin_ios_demo
{
    public partial class ViewController : UIViewController
    {
        // translatedNumber was moved here from ViewDidLoad ()
        string translatedNumber = "";

        public List<string> PhoneNumbers { get; set; }

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            //initialize list of phone numbers called for Call History screen
            PhoneNumbers = new List<string>();

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            translatedNumber = "";

            TranslateButton.TouchUpInside += (object sender, EventArgs e) => {
                // Convert the phone number with text to a number
                // using PhoneTranslator.cs
                translatedNumber = PhoneTranslator.ToNumber(
                    PhoneNumberText.Text);

                // Dismiss the keyboard if text field was tapped
                PhoneNumberText.ResignFirstResponder();

                if (translatedNumber == "")
                {
                    CallButton.SetTitle("Call ", UIControlState.Normal);
                    CallButton.Enabled = false;
                }
                else
                {
                    CallButton.SetTitle("Call " + translatedNumber,
                        UIControlState.Normal);
                    CallButton.Enabled = true;
                }
            };

            CallButton.TouchUpInside += (object sender, EventArgs e) => {



                var idn = new System.Globalization.IdnMapping();
                var dotnetURI = new System.Uri("tel:" + translatedNumber);
                Console.WriteLine(dotnetURI.ToString());
                //NSUrl nsURL = new NSUrl(dotnetURI.Scheme, idn.GetAscii(dotnetURI.DnsSafeHost), dotnetURI.PathAndQuery);
                //Console.WriteLine(nsURL.ToString());
                //WebView.LoadRequest(new NSUrlRequest(nsURL));

                // Use URL handler with tel: prefix to invoke Apple's Phone app...
                //var url = new NSUrl("tel:" + translatedNumber);
                var url = new NSUrl(dotnetURI.Scheme);



                // ...otherwise show an alert dialog
                if (!UIApplication.SharedApplication.OpenUrl(url))
                {
                    var alert = UIAlertController.Create("Not supported", "Scheme 'tel:' is not supported on this device", UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null);
                }

                //Store the phone number that we're dialing in PhoneNumbers
                PhoneNumbers.Add(translatedNumber);
            };


        }



        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            // set the View Controller that’s powering the screen we’re
            // transitioning to

            var callHistoryContoller = segue.DestinationViewController as CallHistoryController;

            //set the Table View Controller’s list of phone numbers to the
            // list of dialed phone numbers

            if (callHistoryContoller != null)
            {
                callHistoryContoller.PhoneNumbers = PhoneNumbers;
            }
        }
    }


}
