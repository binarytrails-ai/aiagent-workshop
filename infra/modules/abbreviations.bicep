// Azure Resource abbreviations as per Microsoft naming conventions
// https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-abbreviations

var resourceAbbreviations = {
  // Core
  resourceGroup: 'rg'
  subscription: 'sub'

  // General
  apiManagement: 'apim'
  appConfiguration: 'appcs'
  appService: 'app'
  appServicePlan: 'plan'
  storageAccount: 'st'

  // AI + Machine Learning
  aiServices: 'ai'
  cognitiveServices: 'cog'
  computerVision: 'cv'
  contentModerator: 'cm'
  machineLearning: 'ml'
  machineLearningSandbox: 'mls'

  // Analytics
  analysisServices: 'as'
  databricks: 'dbw'
  streamAnalytics: 'asa'
  synapse: 'syn'
  synapseAnalytics: 'synw'
  synapseSparkPool: 'synsp'

  // Containers
  containerRegistry: 'cr'
  containerInstance: 'ci'
  kubernetes: 'aks'

  // Databases
  cosmosDb: 'cosmos'
  databaseForPostgreSQL: 'psql'
  databaseForMySQL: 'mysql'
  msSqlDatabase: 'sql'
  msSqlServer: 'sql'

  // Identity
  activeDirectory: 'aad'
  keyVault: 'kv'

  // Integration
  serviceBus: 'sb'
  serviceEndpoint: 'se'
  eventGridTopic: 'evgt'
  eventHub: 'evh'

  // IoT
  iotHub: 'iot'
  iotCentral: 'iotc'
  provisioningServices: 'dps'

  // Management + Governance
  logAnalytics: 'log'
  applicationInsights: 'appi'

  // Networking
  applicationGateway: 'agw'
  bastionHost: 'bas'
  expressRoute: 'erc'
  firewall: 'afw'
  loadBalancer: 'lb'
  networkInterface: 'nic'
  networkSecurityGroup: 'nsg'
  privateEndpoint: 'pep'
  publicIpAddress: 'pip'
  routeTable: 'route'
  virtualNetwork: 'vnet'
  virtualNetworkGateway: 'vgw'
  virtualWan: 'vwan'
}

output abbreviations object = resourceAbbreviations
