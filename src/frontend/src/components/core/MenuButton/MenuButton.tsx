/* eslint-disable react/jsx-props-no-spreading -- This is a wrapper component */
import type { MenuButtonProps as FluentMenuButtonProps, MenuItemProps, MenuListProps, MenuPopoverProps, MenuProps, MenuTriggerProps } from "@fluentui/react-components";
import type { Key } from "react";
import { forwardRef } from "react";
import { MenuButton as FluentMenuButton, Menu, MenuItem, MenuList, MenuPopover, MenuTrigger } from "@fluentui/react-components";
export interface IMenuItemConfig extends MenuItemProps {
  key: Key;
}
export interface IMenuButtonProps {
  menuButtonText: string;
  menuItems: IMenuItemConfig[];
  menuProps?: MenuProps;
  menuButtonProps?: FluentMenuButtonProps;
  menuTriggerProps?: MenuTriggerProps;
  menuListProps?: MenuListProps;
  menuPopoverProps?: MenuPopoverProps;
}
export const MenuButton = forwardRef<HTMLButtonElement, IMenuButtonProps>(
  (
    {
      menuButtonText,
      menuItems,
      menuProps,
      menuButtonProps,
      menuTriggerProps,
      menuListProps,
      menuPopoverProps,
    },
    ref
  ) => (
    <Menu {...menuProps}>
      <MenuTrigger {...menuTriggerProps}>
        <FluentMenuButton ref={ref} {...menuButtonProps}>
          {menuButtonText}
        </FluentMenuButton>
      </MenuTrigger>
      <MenuPopover {...menuPopoverProps}>
        <MenuList {...menuListProps}>
          {menuItems.map(({ key, ...item }) => (
            <MenuItem key={key} {...item} />
          ))}
        </MenuList>
      </MenuPopover>
    </Menu>
  )
);
