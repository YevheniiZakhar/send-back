import { Box, styled } from '@mui/material';
import { observer } from 'mobx-react';
import React from 'react';
import { Outlet } from 'react-router-dom';
import Header from '../Components/Header/Header';
import NavigationMenu from '../Components/Navigation/NavigationMenu';
import { navMenuOpenStore } from '../Stores/NavMenuOpen';

const drawerWidth = 240;

const Main = styled('main', { shouldForwardProp: (prop) => prop !== 'open' })<{
  open?: boolean;
}>(({ theme, open }) => ({
  flexGrow: 1,
  padding: theme.spacing(3),
  transition: theme.transitions.create('margin', {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.leavingScreen,
  }),
  marginLeft: `-${drawerWidth}px`,
  ...(open && {
    transition: theme.transitions.create('margin', {
      easing: theme.transitions.easing.easeOut,
      duration: theme.transitions.duration.enteringScreen,
    }),
    marginLeft: 0,
  }),
}));

const DrawerHeader = styled('div')(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  padding: theme.spacing(0, 1),
  // necessary for content to be below app bar
  ...theme.mixins.toolbar,
  justifyContent: 'flex-end',
}));

const Layout = observer(() => {
  return (
    <Box sx={{ display: 'flex' }}>
      <Header />
      <NavigationMenu
        open={navMenuOpenStore.isOpen}
        handleDrawerClose={() => navMenuOpenStore.toggle()}
      />
      <Main open={navMenuOpenStore.isOpen}>
        <DrawerHeader />
        <Outlet />
      </Main>
      <Box
        component="footer"
        sx={{ alignSelf: 'flex-end', justifySelf: 'flex-end' }}
      >
        Footer
      </Box>
    </Box>
  );
});

export { Layout };
