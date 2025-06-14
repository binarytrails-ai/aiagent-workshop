using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace AIAgent.API.KernelTools
{
    /// <summary>
    /// Define Tech Support Agent functions (tools)
    /// </summary>
    public static class TechSupportTools
    {
        public static string FormattingInstructions => "Instructions: returning the output of this function call verbatim to the user in markdown. Then write AGENT SUMMARY: and then include a summary of what you did.";
        public static string AgentName => "Tech_Support_Agent";

        public static Task<string> SendWelcomeEmail(string employeeName, string emailAddress)
        {
            return Task.FromResult($"##### Welcome Email Sent\n**Employee Name:** {employeeName}\n**Email Address:** {emailAddress}\n\nA welcome email has been successfully sent to {employeeName} at {emailAddress}.\n{FormattingInstructions}");
        }

        public static Task<string> SetUpOffice365Account(string employeeName, string emailAddress)
        {
            return Task.FromResult($"##### Office 365 Account Setup\n**Employee Name:** {employeeName}\n**Email Address:** {emailAddress}\n\nAn Office 365 account has been successfully set up for {employeeName} at {emailAddress}.\n{FormattingInstructions}");
        }

        public static Task<string> ConfigureLaptop(string employeeName, string laptopModel)
        {
            return Task.FromResult($"##### Laptop Configuration\n**Employee Name:** {employeeName}\n**Laptop Model:** {laptopModel}\n\nThe laptop {laptopModel} has been successfully configured for {employeeName}.\n{FormattingInstructions}");
        }

        public static Task<string> ResetPassword(string employeeName)
        {
            return Task.FromResult($"##### Password Reset\n**Employee Name:** {employeeName}\n\nThe password for {employeeName} has been successfully reset.\n{FormattingInstructions}");
        }

        public static Task<string> SetupVpnAccess(string employeeName) =>
            Task.FromResult($"##### VPN Access Setup\n**Employee Name:** {employeeName}\n\nVPN access has been successfully set up for {employeeName}.\n{FormattingInstructions}");

        public static Task<string> TroubleshootNetworkIssue(string issueDescription) =>
            Task.FromResult($"##### Network Issue Resolved\n**Issue Description:** {issueDescription}\n\nThe network issue described as '{issueDescription}' has been successfully resolved.\n{FormattingInstructions}");

        public static Task<string> InstallSoftware(string employeeName, string softwareName) =>
            Task.FromResult($"##### Software Installation\n**Employee Name:** {employeeName}\n**Software Name:** {softwareName}\n\nThe software '{softwareName}' has been successfully installed for {employeeName}.\n{FormattingInstructions}");

        public static Task<string> UpdateSoftware(string employeeName, string softwareName) =>
            Task.FromResult($"##### Software Update\n**Employee Name:** {employeeName}\n**Software Name:** {softwareName}\n\nThe software '{softwareName}' has been successfully updated for {employeeName}.\n{FormattingInstructions}");

        public static Task<string> ManageDataBackup(string employeeName) =>
            Task.FromResult($"##### Data Backup Managed\n**Employee Name:** {employeeName}\n\nData backup has been successfully configured for {employeeName}.\n{FormattingInstructions}");

        public static Task<string> HandleCybersecurityIncident(string incidentDetails) =>
            Task.FromResult($"##### Cybersecurity Incident Handled\n**Incident Details:** {incidentDetails}\n\nThe cybersecurity incident described as '{incidentDetails}' has been successfully handled.\n{FormattingInstructions}");

        public static Task<string> SupportProcurementTech(string equipmentDetails) =>
            Task.FromResult($"##### Technical Specifications Provided\n**Equipment Details:** {equipmentDetails}\n\nTechnical specifications for the following equipment have been provided: {equipmentDetails}.\n{FormattingInstructions}");

        public static Task<string> CollaborateCodeDeployment(string projectName) =>
            Task.FromResult($"##### Code Deployment Collaboration\n**Project Name:** {projectName}\n\nCollaboration on the deployment of project '{projectName}' has been successfully completed.\n{FormattingInstructions}");

        public static Task<string> AssistMarketingTech(string campaignName) =>
            Task.FromResult($"##### Tech Support for Marketing Campaign\n**Campaign Name:** {campaignName}\n\nTechnical support has been successfully provided for the marketing campaign '{campaignName}'.\n{FormattingInstructions}");

        public static Task<string> AssistProductLaunch(string productName) =>
            Task.FromResult($"##### Tech Support for Product Launch\n**Product Name:** {productName}\n\nTechnical support has been successfully provided for the product launch of '{productName}'.\n{FormattingInstructions}");

        public static Task<string> ImplementItPolicy(string policyName) =>
            Task.FromResult($"##### IT Policy Implemented\n**Policy Name:** {policyName}\n\nThe IT policy '{policyName}' has been successfully implemented.\n{FormattingInstructions}");

        public static Task<string> ManageCloudService(string serviceName) =>
            Task.FromResult($"##### Cloud Service Managed\n**Service Name:** {serviceName}\n\nThe cloud service '{serviceName}' has been successfully managed.\n{FormattingInstructions}");

        public static Task<string> ConfigureServer(string serverName) =>
            Task.FromResult($"##### Server Configuration\n**Server Name:** {serverName}\n\nThe server '{serverName}' has been successfully configured.\n{FormattingInstructions}");

        public static Task<string> GrantDatabaseAccess(string employeeName, string databaseName) =>
            Task.FromResult($"##### Database Access Granted\n**Employee Name:** {employeeName}\n**Database Name:** {databaseName}\n\nAccess to the database '{databaseName}' has been successfully granted to {employeeName}.\n{FormattingInstructions}");

        public static Task<string> ProvideTechTraining(string employeeName, string toolName) =>
            Task.FromResult($"##### Tech Training Provided\n**Employee Name:** {employeeName}\n**Tool Name:** {toolName}\n\nTechnical training on '{toolName}' has been successfully provided to {employeeName}.\n{FormattingInstructions}");

        public static Task<string> ResolveTechnicalIssue(string issueDescription) =>
            Task.FromResult($"##### Technical Issue Resolved\n**Issue Description:** {issueDescription}\n\nThe technical issue described as '{issueDescription}' has been successfully resolved.\n{FormattingInstructions}");

        public static Task<string> ConfigurePrinter(string employeeName, string printerModel) =>
            Task.FromResult($"##### Printer Configuration\n**Employee Name:** {employeeName}\n**Printer Model:** {printerModel}\n\nThe printer '{printerModel}' has been successfully configured for {employeeName}.\n{FormattingInstructions}");

        public static Task<string> SetUpEmailSignature(string employeeName, string signature) =>
            Task.FromResult($"##### Email Signature Setup\n**Employee Name:** {employeeName}\n**Signature:** {signature}\n\nThe email signature for {employeeName} has been successfully set up as '{signature}'.\n{FormattingInstructions}");

        public static Task<string> ConfigureMobileDevice(string employeeName, string deviceModel) =>
            Task.FromResult($"##### Mobile Device Configuration\n**Employee Name:** {employeeName}\n**Device Model:** {deviceModel}\n\nThe mobile device '{deviceModel}' has been successfully configured for {employeeName}.\n{FormattingInstructions}");

        public static Task<string> ManageSoftwareLicenses(string softwareName, int licenseCount) =>
            Task.FromResult($"##### Software Licenses Managed\n**Software Name:** {softwareName}\n**License Count:** {licenseCount}\n\n{licenseCount} licenses for the software '{softwareName}' have been successfully managed.\n{FormattingInstructions}");

        public static Task<string> SetUpRemoteDesktop(string employeeName) =>
            Task.FromResult($"##### Remote Desktop Setup\n**Employee Name:** {employeeName}\n\nRemote desktop access has been successfully set up for {employeeName}.\n{FormattingInstructions}");

        public static Task<string> TroubleshootHardwareIssue(string issueDescription) =>
            Task.FromResult($"##### Hardware Issue Resolved\n**Issue Description:** {issueDescription}\n\nThe hardware issue described as '{issueDescription}' has been successfully resolved.\n{FormattingInstructions}");

        public static Task<string> ManageNetworkSecurity() =>
            Task.FromResult($"##### Network Security Managed\n\nNetwork security protocols have been successfully managed.\n{FormattingInstructions}");

        public static Dictionary<string, MethodInfo> GetAllKernelFunctions()
        {
            var kernelFunctions = new Dictionary<string, MethodInfo>();
            var methods = typeof(TechSupportTools).GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
            {
                if (method.Name == nameof(GetAllKernelFunctions))
                    continue;
                kernelFunctions[method.Name] = method;
            }
            return kernelFunctions;
        }
    }
}
