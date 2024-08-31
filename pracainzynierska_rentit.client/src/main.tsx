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
            </Routes>
        </BrowserRouter>
    </MantineProvider>
);
