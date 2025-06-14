// ...existing code from frontend/src/components/core/theme/ThemePicker.tsx...
import React, { useMemo } from "react";
import { Dropdown, Option, Label } from "@fluentui/react-components";
import { Theme, useThemeContext } from "./ThemeContext";
interface IDropdownItem {
  key: Theme;
  value: Theme;
  text: string;
}
export const ThemePicker: React.FC = () => {
  const { savedTheme, setTheme } = useThemeContext();
  const options: IDropdownItem[] = useMemo(
    () => [
      { key: "Light", value: "Light", text: "Light" },
      { key: "Dark", value: "Dark", text: "Dark" },
      { key: "System", value: "System", text: "System" },
    ],
    []
  );
  const selectedThemeText = useMemo(
    () => options.find((opt) => opt.key === (savedTheme ?? "Light"))?.text ?? "Light",
    [savedTheme, options]
  );
  const selectedOptions = useMemo(
    () => (savedTheme ? [savedTheme] : []),
    [savedTheme]
  );
  return (
    <>
      <Label htmlFor="ThemePickerDropdown">Theme</Label>
      <Dropdown
        id="ThemePickerDropdown"
        // ...rest of the code...
      />
    </>
  );
};
