import { faCircleQuestion } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import React from 'react'

export default function ServiceCategoriesCard({category}) {
  return (
      // <div className="card  w-fit p-4 text-center  border border-black/10  rounded-xl py-6 shadow-sm hover:shadow-md transition-shadow duration-300">
      //         <div className="content">
      //              <FontAwesomeIcon icon={faCircleQuestion} />
      //           <h3 className="font-medium">{category}</h3>
      //         </div>
      //       </div>
      <div className="
card 
w-fit 
p-4 py-6 
text-center 
rounded-xl 
border border-gray-200 dark:border-gray-800
bg-white dark:bg-gray-900
shadow-sm dark:shadow-lg
hover:shadow-md dark:hover:shadow-xl
hover:-translate-y-1
transition-all duration-300
">

  <div className="content space-y-2">

    <FontAwesomeIcon 
      icon={faCircleQuestion} 
      className="text-gray-700 dark:text-purple-400 text-lg transition"
    />

    <h3 className="font-medium 
    text-gray-900 dark:text-gray-100">
      {category}
    </h3>

  </div>
</div>
  )
}
