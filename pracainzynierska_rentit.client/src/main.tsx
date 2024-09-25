import '@mantine/core/styles.css';
// @ts-ignore
import React from 'react';
import ReactDOM from 'react-dom/client';
import {
    BrowserRouter,
    Routes,
    Route,
} from 'react-router-dom';
import { MantineProvider, createTheme } from '@mantine/core';
import {Home} from './Home/Home';
import Settings from './Settings/Settings';
import ConfirmationPage from './ConfirmEmail/ConfirmationPage';

const theme = createTheme({

    primaryColor: 'gray',
    focusRing: 'always',
    defaultRadius: 'md',
    defaultGradient: { from: 'blue', to: 'red', deg: 45 }
});
// Routing
// @ts-ignore
ReactDOM.createRoot(document.getElementById('root')).render(
    <MantineProvider theme={theme} forceColorScheme="light">
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/Settings" element={<Settings />} />
                <Route path="/confirm" element={<ConfirmationPage />} />
            </Routes>
        </BrowserRouter>
    </MantineProvider>
);
