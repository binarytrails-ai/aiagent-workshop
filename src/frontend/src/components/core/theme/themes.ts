import { BrandVariants, createDarkTheme, createLightTheme, Theme } from "@fluentui/react-components";

// Define Contoso Bike Store brand colors - using a lighter, fresher biking-themed palette
const contosoBikeColors: BrandVariants = {
  10: "#E9F4FB",
  20: "#D5EAF7",
  30: "#C1E0F3",
  40: "#ADD5EF",
  50: "#8BC6E8",
  60: "#69B7E0",
  70: "#4FA9D9",
  80: "#389AD0",
  90: "#2A8CC2",
  100: "#207EB5",
  110: "#186FA5", 
  120: "#136194",
  130: "#0F5483",
  140: "#0C4672",
  150: "#083862",
  160: "#052B50",
};

// Custom theme additions for bike store
const bikeStoreCustomizations = {
  // Action colors
  colorPaletteRedBackground1: "#FF6B6B", // For important notifications or delete actions - lighter shade
  colorPaletteGreenBackground1: "#66BB6A", // For success states - fresher green 
  colorPaletteOrangeBackground1: "#FFA94D", // For warnings or bike accessory items - vibrant orange
  colorPaletteYellowBackground1: "#FFD43B", // For highlights or promotional items
  
  // UI Borders for a sleeker look
  borderRadiusMedium: "10px",
  borderRadiusLarge: "16px",
};

export const lightTheme: Theme = {
  ...createLightTheme(contosoBikeColors),
  ...bikeStoreCustomizations,
  colorNeutralBackground1: "#FFFFFF",
  colorNeutralBackground2: "#F7FBFF",
  colorNeutralBackground3: "#EEF6FC",
  colorNeutralStroke1: "#D8E8F4",
  colorNeutralForeground1: "#2A3642",
  colorNeutralForeground2: "#586A7B",
  colorBrandForeground1: contosoBikeColors[110],
  colorBrandForeground2: contosoBikeColors[120],
  colorBrandBackground: contosoBikeColors[60],
  colorBrandBackgroundHover: contosoBikeColors[70],
  colorBrandBackgroundPressed: contosoBikeColors[80],
};

export const darkTheme: Theme = {
  ...createDarkTheme(contosoBikeColors),
  ...bikeStoreCustomizations,
  colorBrandForeground1: contosoBikeColors[60],
  colorBrandForeground2: contosoBikeColors[50],
  colorBrandForegroundLink: contosoBikeColors[70],
  colorNeutralBackground1: "#121212",
  colorNeutralBackground2: "#1E1E1E", 
  colorNeutralBackground3: "#252525",
  colorNeutralStroke1: "#3D3D3D",
};
