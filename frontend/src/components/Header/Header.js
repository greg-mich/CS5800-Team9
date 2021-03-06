import React from "react";
import { Link } from "react-router-dom";

import { makeStyles } from "@material-ui/core/styles";

import MenuIcon from "@material-ui/icons/Menu";

import {
  AppBar,
  Toolbar,
  Typography,
  IconButton,
  Button,
  Drawer
} from "@material-ui/core";

import NonAdminLinks from "./lists/NonAdminLinks";
import AdminLinks from "./lists/AdminLinks";
import { getEmail, getRole, isAdmin } from "../../helpers/jwtHelpers";

const drawerWidth = 240;

const useStyles = makeStyles(theme => ({
  grow: {
    flexGrow: 1
  },
  appBar: {
    zIndex: theme.zIndex.drawer + 1
  },
  drawer: {
    width: drawerWidth,
    position: "fixed",
    flexShrink: 0
  },
  drawerPaper: {
    width: drawerWidth
  },
  root: {
    flexGrow: 1
  },
  menuButton: {
    marginRight: theme.spacing(2)
  },
  link: {
    textDecoration: "none",
    color: "black"
  },
  toolbar: theme.mixins.toolbar
}));

export default function Header(props) {
  const classes = useStyles();
  const [drawerState, setDrawerState] = React.useState(false);

  const { clearJWT, jwt } = props;

  const closeDrawer = () => {
    drawerState && setDrawerState(false);
  };

  const toggleDrawer = () => {
    setDrawerState(!drawerState);
  };

  return (
    <div className={classes.root}>
      <AppBar position="fixed" className={classes.appBar}>
        <Toolbar>
          <IconButton
            edge="start"
            className={classes.menuButton}
            color="inherit"
            aria-label="menu"
            onClick={toggleDrawer}
          >
            <MenuIcon />
          </IconButton>
          <Link to="/" className={classes.link}>
            <Typography variant="h6">Classroom</Typography>
          </Link>
          <div className={classes.grow} />
          <Typography variant="subtitle1" style={{ paddingRight: ".4em" }}>
            {getEmail(jwt)}
          </Typography>
          <Typography variant="subtitle1" style={{ paddingRight: ".8em" }}>
            {getRole(jwt) ? `(${getRole(jwt)})` : ""}
          </Typography>
          <Button variant="contained" color="secondary" onClick={clearJWT}>
            Sign Out
          </Button>
        </Toolbar>
      </AppBar>
      <Drawer
        className={classes.drawer}
        open={drawerState}
        onClose={closeDrawer}
        classes={{
          paper: classes.drawerPaper
        }}
        style={{ zIndex: 100 }}
      >
        <div className={classes.toolbar} />
        {isAdmin() && <AdminLinks closeDrawer={closeDrawer} />}
        {!isAdmin() && <NonAdminLinks closeDrawer={closeDrawer} />}
      </Drawer>
    </div>
  );
}
