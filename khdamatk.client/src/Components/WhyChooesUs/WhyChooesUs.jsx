import { faBolt, faCircleCheck, faLock, faScrewdriverWrench } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import React from 'react'

export default function () {
  return (
    <div>
    <section className="bg-gray-50 py-16">
  <div className="container mx-auto px-4">

    <h2 className="text-3xl font-bold text-center mb-12 text-gray-800">
      Why Create an Account with KHADMA HUB?
    </h2>

    <div className="grid grid-cols-1 gap-8 sm:grid-cols-2 lg:grid-cols-4">
      
      <div className="bg-white p-6 rounded-xl shadow text-center space-y-4">
        <div className="w-12 h-12 mx-auto flex items-center justify-center rounded-full bg-purple-100 text-purple-700">
          <FontAwesomeIcon icon={faBolt} />
        </div>
        <h3 className="font-semibold text-lg">Fast & Easy Booking</h3>
        <p className="text-sm text-gray-600">
          Book the service you need in minutes with simple steps.
        </p>
      </div>

      <div className="bg-white p-6 rounded-xl shadow text-center space-y-4">
        <div className="w-12 h-12 mx-auto flex items-center justify-center rounded-full bg-purple-100 text-purple-700">
          <FontAwesomeIcon icon={faCircleCheck} />
        </div>
        <h3 className="font-semibold text-lg">Verified Providers</h3>
        <p className="text-sm text-gray-600">
          All service providers are reviewed for quality and safety.
        </p>
      </div>

      <div className="bg-white p-6 rounded-xl shadow text-center space-y-4">
        <div className="w-12 h-12 mx-auto flex items-center justify-center rounded-full bg-purple-100 text-purple-700">
          <FontAwesomeIcon icon={faScrewdriverWrench} />
        </div>
        <h3 className="font-semibold text-lg">Multiple Services</h3>
        <p className="text-sm text-gray-600">
          Home services, maintenance, cleaning, and more.
        </p>
      </div>

      <div className="bg-white p-6 rounded-xl shadow text-center space-y-4">
        <div className="w-12 h-12 mx-auto flex items-center justify-center rounded-full bg-purple-100 text-purple-700">
          <FontAwesomeIcon icon={faLock} />
        </div>
        <h3 className="font-semibold text-lg">Secure & Reliable</h3>
        <p className="text-sm text-gray-600">
          Your data and bookings are fully protected.
        </p>
      </div>

    </div>

  </div>
</section>

    </div>
  )
}
