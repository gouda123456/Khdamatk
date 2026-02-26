import { faBell, faCircleUser, faLightbulb, faMessage, faMoon, faStar, faSun, faUser, faXmarkCircle } from '@fortawesome/free-regular-svg-icons';
import { faBars, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useContext, useState } from 'react'
import { Link, NavLink } from 'react-router-dom';
import { AuthContext } from './../Context/AuthContext';
import { ThemeContext } from '../Context/Theme.Context';

export default function Navbar() {
  const {theme,toggleTheme}=useContext(ThemeContext)
  const[isMenueOpen,setIsMenueOpen]=useState(false)
  const {logOut}=useContext(AuthContext)
  function openMenue(){
    setIsMenueOpen(! isMenueOpen)
  }
  function closeMenue(){
  setIsMenueOpen(false)
}

  return (
    <>
    <div className=' flex items-center justify-between bg-white shadow fixed top-0 left-0 w-full z-50  p-4  dark:text-gray-200 dark:bg-gray-950 dark:border-gray-800'>
              <Link to="/" className='text-Purple-400 font-bold text-2xl'>KHADMA <span className='text-amber-500'> HUB</span></Link>
              <div className=' hidden lg:flex items-center gap-8'>
                <button className=' flex items-center gap-2'>
                  <span>Services</span>
                 
                </button>
                
                <span>About</span>
                <span>Job</span>
              </div >
              <div className='hidden lg:flex items-center gap-8 text-2xl'>
                <FontAwesomeIcon icon={faMessage} />
                <FontAwesomeIcon icon={faBell} />
                <span>AR</span>
               
<div className='relative group inline-block'>
  <button>
    <FontAwesomeIcon className='text-2xl' icon={faCircleUser} />
  </button>

  <div className='absolute right-0 top-full w-44 bg-white shadow-lg rounded-lg 
                  opacity-0 invisible group-hover:opacity-100 
                  group-hover:visible transition duration-200'>

   
    <ul className='divide-y divide-gray-200 text-sm'>
  <li>
    <Link 
      to="/profile" 
      className='block w-full px-4 py-2 hover:bg-gray-100'
    >
      Profile
    </Link>
  </li>

  <li>
    <Link 
      to="/wallet" 
      className='block w-full px-4 py-2 hover:bg-gray-100'
    >
      Wallet
    </Link>
  </li>

  <li>
    <Link 
      to="/my-offers" 
      className='block w-full px-4 py-2 hover:bg-gray-100'
    >
      My offers
    </Link>
  </li>

 <li>
  <button
    onClick={logOut}
    className='block w-full text-left px-4 py-2 hover:bg-red-50 text-red-600'
  >
    Log out
  </button>
</li>
</ul>


  </div>
</div>


                <button onClick={toggleTheme}>
                  <FontAwesomeIcon icon={theme === 'dark' ?  faSun : faMoon} />
                </button>
              
              </div>
              <button onClick={openMenue} className='lg:hidden btn w-fit text-white'>
              
              {isMenueOpen ?   <FontAwesomeIcon icon={faXmark} /> :  <FontAwesomeIcon icon={faBars} />}
              </button>
              
    </div>
    
   {isMenueOpen && (
  <>
    {/* Overlay */}
    <div 
      onClick={openMenue}
      className="lg:hidden fixed z-30 inset-0 bg-black/30 cursor-pointer"
    ></div>

    {/* Side Menu*/}
  <div className="lg:hidden fixed z-40 bg-white top-0 bottom-0 left-0 w-80 shadow-xl transition-transform duration-300 animate-side-in">
      
      <div className='flex items-center justify-between p-4 border-b'>
        <Link to="/" className='text-purple-500 font-bold  lg:text-2xl'>
          KHADMA <span className='text-amber-500'>HUB</span>
        </Link>
        <button 
          onClick={openMenue}
          className=" text-gray-600 btn bg-gray-100 w-fit cursor-pointer"
        >
          <FontAwesomeIcon icon={faXmark} />
        </button>
              

      </div>
 
 

      {/* Links */}
      <div className='mt-4 border-b'>
        <ul className='divide-y divide-gray-200 text-sm'>
          <li>
            <Link  onClick={closeMenue} to="/" className='block px-4 py-3 hover:bg-gray-100'>
              Services
            </Link>
          </li>

          <li>
            <Link  onClick={closeMenue} to="/about" className='block px-4 py-3 hover:bg-gray-100'>
              About
            </Link>
          </li>

          <li>
            <Link  onClick={closeMenue} to="/job" className='block px-4 py-3 hover:bg-gray-100'>
              Job
            </Link>
          </li>
        </ul>
      </div>

      {/* User Links */}
      <div className='mt-4'>
        <ul className='divide-y divide-gray-200 text-sm'>
          <li>
            <Link  onClick={closeMenue} to="/profile" className='block px-4 py-3 hover:bg-gray-100'>
              Profile
            </Link>
          </li>

          <li>
            <Link  onClick={closeMenue} to="/wallet" className='block px-4 py-3 hover:bg-gray-100'>
              Wallet
            </Link>
          </li>

          <li>
            <Link  onClick={closeMenue} to="/my-offers" className='block px-4 py-3 hover:bg-gray-100'>
              My offers
            </Link>
          </li>

          <li>
            <button
              onClick={logOut}
              className='w-full text-left px-4 py-3 hover:bg-red-50 text-red-600'
            >
              Log out
            </button>
          </li>
        </ul>
      </div>

    </div>
  </>
)}

    
    
    </>
  
)

  
}
