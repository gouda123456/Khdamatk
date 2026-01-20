import React from 'react';
import Signup from './Pages/Signup/Signup';
import Login from './Pages/Login/Login';
import ForgetPassword from './Pages/ForgetPassword/ForgetPassword';
import VerifyCode from './Pages/VerifyCode/VerifyCode';
import SetNewPassword from './Pages/SetNewPassword/SetNewPassword';
import { ToastContainer } from 'react-toastify';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import Layout from './Components/Layout/Layout';
import Home from './Pages/Home/Home';
import NotFound from './Pages/NotFound/NotFound';
import ConfirmEmail from './Pages/ConfirmEmail/ConfirmEmail';

function App() {
    const router=createBrowserRouter([
        {
            path:'/',
            element:<Layout/>,
            children:[
              {
                   index: true,
                   element:<Home/>
              },
              {
                path:'signup',
                element:<Signup/>
              },
              {
                path:'login',
                element:<Login/>
              },
              {
                path:'forget-password',
                element:<ForgetPassword/>
              },
              {
                path:'verify-code',
                element:<VerifyCode/>
              },
              {
                path:'set-new-password',
                element:<SetNewPassword/>
              },
               {
                path:'confirm-email',
                element:<ConfirmEmail/>
              },
              {
                path:'*',
                element:<NotFound/>

              }

            ]
        }
    ])
    return (
        <>
            {/* <Signup/> */}
            {/* <Login/> */}
            {/* <ForgetPassword/> */}
            {/* <VerifyCode/> */}
            {/* <SetNewPassword/> */}

            <RouterProvider router={router}/>
             <ToastContainer position='top-right' autoClose={3000}  closeButton={false}  closeOnClick={true} />
        
        </>
    );
}

export default App;

