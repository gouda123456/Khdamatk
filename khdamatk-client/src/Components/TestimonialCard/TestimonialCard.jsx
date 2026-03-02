import photo from '../../assets/Images/Logo.png'

export default function TestimonialCard({data}) {
    const {clientName,jobtitle
,rating
,
reviewText
}=data
  return (
     <div>
        <div className=" flex items-center justify-center">
            <div className="card w-72 text-center border border-gray-100 dark:border-gray-800
shadow-sm dark:shadow-lg rounded-2xl p-6   hover:-translate-y-2 
hover:shadow-xl 
transition-all duration-300 ">
               
                <div>
                      <img src={photo} className='size-14 object-cover rounded-full mx-auto mb-3'/>
                    <h2 className='text-lg font-semibold text-black dark:text-white'>{clientName}</h2>
                    <h3 className='text-sm text-gray-500 dark:text-white'>{jobtitle}</h3>
                    <p className='text-sm text-black leading-relaxed mt-5 dark:text-whitecd khdamatk.client'>{reviewText}</p>
                </div>
            </div>
        </div>









     </div>
  )
}
