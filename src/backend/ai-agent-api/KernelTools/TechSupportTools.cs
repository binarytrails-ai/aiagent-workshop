using System;
using Microsoft.SemanticKernel;

namespace AIAgent.API.KernelTools
{
    /// <summary>
    /// Define Tech Support Agent functions (tools)
    /// </summary>
    public class TechSupportTools
    {
        private static readonly Random _rand = new Random();
        public static string FormattingInstructions => "Instructions: returning the output of this function call verbatim to the user in markdown. Then write AGENT SUMMARY: and then include a summary of what you did.";

        private string GetRandomStatus(string[] statuses)
        {
            return statuses[_rand.Next(statuses.Length)];
        }

        private string GenerateTicketNumber() => $"TS-{_rand.Next(100000, 999999)}";

        [KernelFunction]
        public Task<string> SendWelcomeEmail(string employeeName, string emailAddress)
        {
            var status = GetRandomStatus(new[] {
                $"A welcome email has been successfully sent to {employeeName} at {emailAddress}.",
                $"Failed to send welcome email to {employeeName}. A support request has been raised. Ticket: {GenerateTicketNumber()}.",
                $"Welcome email to {employeeName} is queued and will be sent shortly."
            });
            return Task.FromResult($"##### Welcome Email Status\n**Employee Name:** {employeeName}\n**Email Address:** {emailAddress}\n\n{status}\n(Mocked Response)\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> SetUpOffice365Account(string employeeName, string emailAddress)
        {
            var status = GetRandomStatus(new[] {
                $"Office 365 account has been successfully set up for {employeeName}.",
                $"Office 365 account setup for {employeeName} is in progress.",
                $"Failed to set up Office 365 account for {employeeName}. A support request has been raised. Ticket: {GenerateTicketNumber()}"
            });
            return Task.FromResult($"##### Office 365 Account Setup\n**Employee Name:** {employeeName}\n**Email Address:** {emailAddress}\n\n{status}\n(Mocked Response)\n{FormattingInstructions}");
        }


        [KernelFunction]
        public Task<string> ResetPassword(string employeeName)
        {
            var status = GetRandomStatus(new[] {
                $"The password for {employeeName} has been successfully reset.",
                $"Password reset for {employeeName} is pending approval.",
                $"Failed to reset password for {employeeName}. A support request has been raised. Ticket: {GenerateTicketNumber()}"
            });
            return Task.FromResult($"##### Password Reset\n**Employee Name:** {employeeName}\n\n{status}\n(Mocked Response)\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> SetupVpnAccess(string employeeName)
        {
            var status = GetRandomStatus(new[] {
                $"VPN access has been successfully set up for {employeeName}.",
                $"VPN setup for {employeeName} is in progress.",
                $"Failed to set up VPN access for {employeeName}. A support request has been raised. Ticket: {GenerateTicketNumber()}"
            });
            return Task.FromResult($"##### VPN Access Setup\n**Employee Name:** {employeeName}\n\n{status}\n(Mocked Response)\n{FormattingInstructions}");
        }


        [KernelFunction]
        public Task<string> InstallSoftware(string employeeName, string softwareName)
        {
            var status = GetRandomStatus(new[] {
                $"The software '{softwareName}' has been successfully installed for {employeeName}.",
                $"Installation of '{softwareName}' for {employeeName} is pending license approval.",
                $"Failed to install '{softwareName}' for {employeeName}. A support request has been raised. Ticket: {GenerateTicketNumber()}"
            });
            return Task.FromResult($"##### Software Installation\n**Employee Name:** {employeeName}\n**Software Name:** {softwareName}\n\n{status}\n(Mocked Response)\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> UpdateSoftware(string employeeName, string softwareName)
        {
            var status = GetRandomStatus(new[] {
                $"The software '{softwareName}' has been successfully updated for {employeeName}.",
                $"Update for '{softwareName}' is scheduled for {employeeName}.",
                $"Failed to update '{softwareName}' for {employeeName}. A support request has been raised. Ticket: {GenerateTicketNumber()}"
            });
            return Task.FromResult($"##### Software Update\n**Employee Name:** {employeeName}\n**Software Name:** {softwareName}\n\n{status}\n(Mocked Response)\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> ManageDataBackup(string employeeName)
        {
            var status = GetRandomStatus(new[] {
                $"Data backup has been successfully configured for {employeeName}.",
                $"Data backup for {employeeName} is scheduled for tonight.",
                $"Failed to configure data backup for {employeeName}. A support request has been raised. Ticket: {GenerateTicketNumber()}"
            });
            return Task.FromResult($"##### Data Backup Status\n**Employee Name:** {employeeName}\n\n{status}\n(Mocked Response)\n{FormattingInstructions}");
        }


        [KernelFunction]
        public Task<string> SupportProcurementTech(string equipmentDetails)
        {
            var status = GetRandomStatus(new[] {
                $"Technical specifications for the following equipment have been provided: {equipmentDetails}.",
                $"Pending approval for the technical specifications of: {equipmentDetails}.",
                $"Failed to provide specifications for {equipmentDetails}. Item not found in inventory. A support request has been raised. Ticket: {GenerateTicketNumber()}"
            });
            return Task.FromResult($"##### Technical Specifications Status\n**Equipment Details:** {equipmentDetails}\n\n{status}\n(Mocked Response)\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> ConfigurePrinter(string employeeName, string printerModel)
        {
            var status = GetRandomStatus(new[] {
                $"The printer '{printerModel}' has been successfully configured for {employeeName}.",
                $"Configuration of printer '{printerModel}' for {employeeName} is pending.",
                $"Failed to configure printer '{printerModel}' for {employeeName}. Printer not found. A support request has been raised. Ticket: {GenerateTicketNumber()}"
            });
            return Task.FromResult($"##### Printer Configuration Status\n**Employee Name:** {employeeName}\n**Printer Model:** {printerModel}\n\n{status}\n(Mocked Response)\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> ManageSoftwareLicenses(string softwareName, int licenseCount)
        {
            var status = GetRandomStatus(new[] {
                $"{licenseCount} licenses for the software '{softwareName}' have been successfully managed.",
                $"Management of {licenseCount} licenses for '{softwareName}' is under review.",
                $"Failed to manage licenses for '{softwareName}'. License server unreachable. A support request has been raised. Ticket: {GenerateTicketNumber()}"
            });
            return Task.FromResult($"##### Software Licenses Management Status\n**Software Name:** {softwareName}\n**License Count:** {licenseCount}\n\n{status}\n(Mocked Response)\n{FormattingInstructions}");
        }


        [KernelFunction]
        public Task<string> ManageNetworkSecurity()
        {
            var status = GetRandomStatus(new[] {
                $"Network security protocols have been successfully managed.",
                $"Network security management is under review.",
                $"Failed to manage network security. Immediate attention required. A support request has been raised. Ticket: {GenerateTicketNumber()}"
            });
            return Task.FromResult($"##### Network Security Management Status\n\n{status}\n(Mocked Response)\n{FormattingInstructions}");
        }
    }
}
