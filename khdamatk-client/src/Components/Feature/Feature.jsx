import { faClock, faLayerGroup, faShieldHalved, faSliders } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import React from 'react'

export default function Feature() {
  return (
    
//   <div className='bg-gray-50'>
//       <div className="container space-y-6 p-10">
//     <h2 className='font-bold text-3xl text-center'> Features</h2>
    
//   <div className="grid grid-cols-1 md:grid-cols-2 gap-6">

//     <div className="flex gap-3 items-start ">
//       <FontAwesomeIcon icon={faLayerGroup} className="text-xl " />
//       <div>
//         <h2 className="font-bold">Centralized Services</h2>
//         <p>Access all services in one place through a unified and easy-to-use platform that saves time and effort.</p>
//       </div>
//     </div>

//     <div className="flex gap-3 items-start">
//       <FontAwesomeIcon icon={faSliders} className="text-xl" />
//       <div>
//         <h2 className="font-bold">Smart Service Matching</h2>
//         <p>Find the right service quickly with intelligent filtering and tailored recommendations.</p>
//       </div>
//     </div>

//     <div className="flex gap-3 items-start">
//       <FontAwesomeIcon icon={faShieldHalved} className="text-xl " />
//       <div>
//         <h2 className="font-bold">Secure & Reliable</h2>
//         <p>Your data and transactions are protected with high security standards.</p>
//       </div>
//     </div>

//     <div className="flex gap-3 items-start">
//       <FontAwesomeIcon icon={faClock} className="text-xl " />
//       <div>
//         <h2 className="font-bold">24/7 Availability</h2>
//         <p>Services are available anytime, anywhere.</p>
//       </div>
//     </div>

//   </div>
// </div>
//   </div>
<div className="bg-gray-50 dark:bg-gray-900 transition-colors duration-300">

  <div className="container space-y-8 p-10">

    <h2 className="font-bold text-3xl text-center 
    text-gray-900 dark:text-gray-100">
      Features
    </h2>
    
    <div className="grid grid-cols-1 md:grid-cols-2 gap-8">

      {/* Feature Item */}
      <div className="flex gap-4 items-start 
      p-4 rounded-xl
      hover:bg-gray-100 dark:hover:bg-gray-800
      transition">

        <FontAwesomeIcon 
          icon={faLayerGroup} 
          className="text-xl text-purple-600 dark:text-purple-400 mt-1" 
        />

        <div>
          <h3 className="font-bold 
          text-gray-900 dark:text-gray-100">
            Centralized Services
          </h3>

          <p className="text-gray-600 dark:text-gray-400">
            Access all services in one place through a unified and easy-to-use platform that saves time and effort.
          </p>
        </div>

      </div>

      {/* Smart Matching */}
      <div className="flex gap-4 items-start 
      p-4 rounded-xl
      hover:bg-gray-100 dark:hover:bg-gray-800
      transition">

        <FontAwesomeIcon 
          icon={faSliders} 
          className="text-xl text-purple-600 dark:text-purple-400 mt-1" 
        />

        <div>
          <h3 className="font-bold 
          text-gray-900 dark:text-gray-100">
            Smart Service Matching
          </h3>

          <p className="text-gray-600 dark:text-gray-400">
            Find the right service quickly with intelligent filtering and tailored recommendations.
          </p>
        </div>

      </div>

      {/* Secure */}
      <div className="flex gap-4 items-start 
      p-4 rounded-xl
      hover:bg-gray-100 dark:hover:bg-gray-800
      transition">

        <FontAwesomeIcon 
          icon={faShieldHalved} 
          className="text-xl text-purple-600 dark:text-purple-400 mt-1" 
        />

        <div>
          <h3 className="font-bold 
          text-gray-900 dark:text-gray-100">
            Secure & Reliable
          </h3>

          <p className="text-gray-600 dark:text-gray-400">
            Your data and transactions are protected with high security standards.
          </p>
        </div>

      </div>

      {/* 24/7 */}
      <div className="flex gap-4 items-start 
      p-4 rounded-xl
      hover:bg-gray-100 dark:hover:bg-gray-800
      transition">

        <FontAwesomeIcon 
          icon={faClock} 
          className="text-xl text-purple-600 dark:text-purple-400 mt-1" 
        />

        <div>
          <h3 className="font-bold 
          text-gray-900 dark:text-gray-100">
            24/7 Availability
          </h3>

          <p className="text-gray-600 dark:text-gray-400">
            Services are available anytime, anywhere.
          </p>
        </div>

      </div>

    </div>
  </div>

</div>


  )
}
