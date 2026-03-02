import React, { useEffect, useState } from 'react'
import TestimonialCard from '../TestimonialCard/TestimonialCard'
import Loading from '../Loading/Loading'
import { getHomeData } from '../../Services/home-api'

export default function ClientReviews() {
    const[clientReviews,setClientReviews]=useState([])
        const[loading,setLoading]=useState(true)
         async function getClientReviews(){
            try {
                        const response= await getHomeData()
                        if(response.isSuccess){
                            setLoading(false)
                            setClientReviews(response.data.clientReviews)
                        }
                    } catch (error) {
                        setLoading(true)
                        console.log(error)
                        
                    }
        }
        useEffect(()=>{
           getClientReviews()
        },[])
        if(loading){
          return <Loading/>
        }
  return (
    <div className='dark:bg-gray-900 dark:text-white bg-gray-50'>
          <div className="container space-y-4 p-10">
        <h2 className='font-bold text-3xl text-center'>Hear from Our Clients</h2>
         {/* details-card */}
    <div className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4'>
     
      {clientReviews.map((item, index) => (
  <TestimonialCard key={index} data={item} />
))}
       
  
  

    </div>
     </div>
    </div>
    
  )
}
