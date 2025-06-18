using Microsoft.SemanticKernel;

namespace AIAgent.API.KernelTools
{
    /// <summary>
    /// Define Tech Support Agent functions (tools)
    /// </summary>
    public class TechSupportTools
    {
        public static string FormattingInstructions => "Instructions: returning the output of this function call verbatim to the user in markdown. Then write AGENT SUMMARY: and then include a summary of what you did.";

        [KernelFunction]
        public Task<string> SendWelcomeEmail(string employeeName, string emailAddress)
        {
            return Task.FromResult($"##### Welcome Email Sent\n**Employee Name:** {employeeName}\n**Email Address:** {emailAddress}\n\nA welcome email has been successfully sent to {employeeName} at {emailAddress}.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> SetUpOffice365Account(string employeeName, string emailAddress)
        {
            return Task.FromResult($"##### Office 365 Account Setup\n**Employee Name:** {employeeName}\n**Email Address:** {emailAddress}\n\nAn Office 365 account has been successfully set up for {employeeName} at {emailAddress}.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> ConfigureLaptop(string employeeName, string laptopModel)
        {
            return Task.FromResult($"##### Laptop Configuration\n**Employee Name:** {employeeName}\n**Laptop Model:** {laptopModel}\n\nThe laptop {laptopModel} has been successfully configured for {employeeName}.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> ResetPassword(string employeeName)
        {
            return Task.FromResult($"##### Password Reset\n**Employee Name:** {employeeName}\n\nThe password for {employeeName} has been successfully reset.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> SetupVpnAccess(string employeeName)
        {
            return Task.FromResult($"##### VPN Access Setup\n**Employee Name:** {employeeName}\n\nVPN access has been successfully set up for {employeeName}.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> TroubleshootNetworkIssue(string issueDescription)
        {
            return Task.FromResult($"##### Network Issue Resolved\n**Issue Description:** {issueDescription}\n\nThe network issue described as '{issueDescription}' has been successfully resolved.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> InstallSoftware(string employeeName, string softwareName)
        {
            return Task.FromResult($"##### Software Installation\n**Employee Name:** {employeeName}\n**Software Name:** {softwareName}\n\nThe software '{softwareName}' has been successfully installed for {employeeName}.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> UpdateSoftware(string employeeName, string softwareName)
        {
            return Task.FromResult($"##### Software Update\n**Employee Name:** {employeeName}\n**Software Name:** {softwareName}\n\nThe software '{softwareName}' has been successfully updated for {employeeName}.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> ManageDataBackup(string employeeName)
        {
            return Task.FromResult($"##### Data Backup Managed\n**Employee Name:** {employeeName}\n\nData backup has been successfully configured for {employeeName}.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> HandleCybersecurityIncident(string incidentDetails)
        {
            return Task.FromResult($"##### Cybersecurity Incident Handled\n**Incident Details:** {incidentDetails}\n\nThe cybersecurity incident described as '{incidentDetails}' has been successfully handled.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> SupportProcurementTech(string equipmentDetails)
        {
            return Task.FromResult($"##### Technical Specifications Provided\n**Equipment Details:** {equipmentDetails}\n\nTechnical specifications for the following equipment have been provided: {equipmentDetails}.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> AssistMarketingTech(string campaignName)
        {
            return Task.FromResult($"##### Tech Support for Marketing Campaign\n**Campaign Name:** {campaignName}\n\nTechnical support has been successfully provided for the marketing campaign '{campaignName}'.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> AssistProductLaunch(string productName)
        {
            return Task.FromResult($"##### Tech Support for Product Launch\n**Product Name:** {productName}\n\nTechnical support has been successfully provided for the product launch of '{productName}'.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> ImplementItPolicy(string policyName)
        {
            return Task.FromResult($"##### IT Policy Implemented\n**Policy Name:** {policyName}\n\nThe IT policy '{policyName}' has been successfully implemented.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> ManageCloudService(string serviceName)
        {
            return Task.FromResult($"##### Cloud Service Managed\n**Service Name:** {serviceName}\n\nThe cloud service '{serviceName}' has been successfully managed.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> ConfigureServer(string serverName)
        {
            return Task.FromResult($"##### Server Configuration\n**Server Name:** {serverName}\n\nThe server '{serverName}' has been successfully configured.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> GrantDatabaseAccess(string employeeName, string databaseName)
        {
            return Task.FromResult($"##### Database Access Granted\n**Employee Name:** {employeeName}\n**Database Name:** {databaseName}\n\nAccess to the database '{databaseName}' has been successfully granted to {employeeName}.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> ProvideTechTraining(string employeeName, string toolName)
        {
            return Task.FromResult($"##### Tech Training Provided\n**Employee Name:** {employeeName}\n**Tool Name:** {toolName}\n\nTechnical training on '{toolName}' has been successfully provided to {employeeName}.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> ResolveTechnicalIssue(string issueDescription)
        {
            return Task.FromResult($"##### Technical Issue Resolved\n**Issue Description:** {issueDescription}\n\nThe technical issue described as '{issueDescription}' has been successfully resolved.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> ConfigurePrinter(string employeeName, string printerModel)
        {
            return Task.FromResult($"##### Printer Configuration\n**Employee Name:** {employeeName}\n**Printer Model:** {printerModel}\n\nThe printer '{printerModel}' has been successfully configured for {employeeName}.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> SetUpEmailSignature(string employeeName, string signature)
        {
            return Task.FromResult($"##### Email Signature Setup\n**Employee Name:** {employeeName}\n**Signature:** {signature}\n\nThe email signature for {employeeName} has been successfully set up as '{signature}'.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> ConfigureMobileDevice(string employeeName, string deviceModel)
        {
            return Task.FromResult($"##### Mobile Device Configuration\n**Employee Name:** {employeeName}\n**Device Model:** {deviceModel}\n\nThe mobile device '{deviceModel}' has been successfully configured for {employeeName}.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> ManageSoftwareLicenses(string softwareName, int licenseCount)
        {
            return Task.FromResult($"##### Software Licenses Managed\n**Software Name:** {softwareName}\n**License Count:** {licenseCount}\n\n{licenseCount} licenses for the software '{softwareName}' have been successfully managed.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> SetUpRemoteDesktop(string employeeName)
        {
            return Task.FromResult($"##### Remote Desktop Setup\n**Employee Name:** {employeeName}\n\nRemote desktop access has been successfully set up for {employeeName}.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> TroubleshootHardwareIssue(string issueDescription)
        {
            return Task.FromResult($"##### Hardware Issue Resolved\n**Issue Description:** {issueDescription}\n\nThe hardware issue described as '{issueDescription}' has been successfully resolved.\n{FormattingInstructions}");
        }

        [KernelFunction]
        public Task<string> ManageNetworkSecurity()
        {
            return Task.FromResult($"##### Network Security Managed\n\nNetwork security protocols have been successfully managed.\n{FormattingInstructions}");
        }
    }
}
