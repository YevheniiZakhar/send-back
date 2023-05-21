import { Menu, MenuItem } from '@mui/material';
import React from 'react';

interface IMenuProps {
  anchorEl: HTMLElement | null;
  isMenuOpen: boolean;
  handleMenuClose: () => void;
}

const menuId = 'primary-search-account-menu';

const RenderMenu = (props: IMenuProps) => {
  const { anchorEl, isMenuOpen, handleMenuClose } = props;
  return (
    <Menu
      anchorEl={anchorEl}
      anchorOrigin={{
        vertical: 'top',
        horizontal: 'right',
      }}
      id={menuId}
      keepMounted
      transformOrigin={{
        vertical: 'top',
        horizontal: 'right',
      }}
      open={isMenuOpen}
      onClose={handleMenuClose}
    >
      <MenuItem onClick={handleMenuClose}>Profile</MenuItem>
      <MenuItem onClick={handleMenuClose}>My account</MenuItem>
    </Menu>
  );
};

export { RenderMenu };
export type { IMenuProps };
