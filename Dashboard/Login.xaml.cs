/******************************************************************************
 * Filename    = Login.xaml.cs
 *
 * Author      = Aravind Somaraj
 *
 * Product     = Analyzer
 * 
 * Project     = Dashboard
 *
 * Description = Defines the Login page.
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Logging;
using Networking.Utils;
using Logging;
using System.Runtime.CompilerServices;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Login"/> class.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <param name="userEmail">The email of the user.</param>
        /// <param name="userPicture">The picture of the user.</param>
        public Login( string userName , string userEmail , string userPicture )
        {
            InitializeComponent();
            UserName = userName;
            UserId = userEmail;
            UserImage = userPicture;
            DataContext = this; // Set the DataContext to this instance
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        public string UserName { get; init; }

        /// <summary>
        /// Gets the email of the user.
        /// </summary>
        public string UserId { get; init; }

        /// <summary>
        /// Gets the user image of the instructor.
        /// </summary>
        public string UserImage { get; init; }

        /// <summary>
        /// Handles the click event for the Instructor button.
        /// Navigates to the InstructorPage.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        private void InstructorButton_Click(object sender, RoutedEventArgs e)
        {
            InstructorPage instructorPage = new( UserName,UserId,UserImage );
            Logger.Inform( $"[LoginPage] Created new InstructorPage : #{RuntimeHelpers.GetHashCode( instructorPage )}" );
            NavigationService?.Navigate( instructorPage );
        }

        /// <summary>
        /// Handles the click event for the Student button.
        /// Navigates to the StudentPage.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        private void StudentButton_Click( object sender , RoutedEventArgs e )
        {

            string insIP = InsIP.Text;
            string insPort = InsPort.Text;

            StudentPage studentPage = new( UserName,UserId,UserImage,insIP,insPort );

            Logger.Inform($"[LoginPage] Created new StudentPage : #{RuntimeHelpers.GetHashCode( studentPage )}" );
            NavigationService?.Navigate( studentPage );
        }

    }
}
