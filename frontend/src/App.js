import React from "react";

import { createMuiTheme, responsiveFontSizes } from "@material-ui/core/styles";
import { ThemeProvider } from "@material-ui/styles";

import CssBaseline from "@material-ui/core/CssBaseline";

import MainView from "./views/MainView";
import StudentIndex from "./views/StudentIndex/StudentIndex";

import Header from "./components/Header/Header";

let theme = createMuiTheme({
  palette: {
    primary: {
      main: "#FFF796"
    },
    background: {
      default: "#FFFDEB"
    }
  }
});
theme = responsiveFontSizes(theme);

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Header />
      <MainView>
        <StudentIndex />
      </MainView>
    </ThemeProvider>
  );
}

export default App;