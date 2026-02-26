import React from 'react'
import { useContext } from 'react'
import { AuthContext } from '../Context/AuthContext';
import { Navigate, useLocation } from 'react-router-dom';

export default function ProtectedRout({ children }) {
  const location=useLocation()

  const {token}=useContext(AuthContext)
  if(token === null){
    return <Navigate to="/login" state={{from: location.pathname}}/>
  }
  else{
   return children;

    
  }

}
