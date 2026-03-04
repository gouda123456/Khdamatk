import React, { useEffect, useState } from 'react'
import FreelancerCard from '../FreelancerCard/FreelancerCard'
import { getHomeData } from '../../Services/home-api'
import Loading from '../Loading/Loading'

export default function Freelancer() {
    const[freelancer,setFreelancer]=useState([])
    const[loading,setLoading]=useState(true)
     async function getFreelancerDetails(){
        try {
                    const response= await getHomeData()
                    if(response.isSuccess){
                        setLoading(false)
                        setFreelancer(response.data.providers)
                    }
                } catch (error) {
                    setLoading(true)
                    console.log(error)
                    
                }
    }
    useEffect(()=>{
       getFreelancerDetails() 
    },[])
    if(loading){
      return <Loading/>
    }
  return (
    <div className='dark:bg-gray-950 dark:text-white'>
       <div className="container space-y-4  p-10 ">
         <h2 className='font-bold text-3xl text-center'>Our Top-Rated Freelancers</h2>
          <div className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4'>
            {freelancer.map((items)=>(
               <FreelancerCard key={items.id} data={items} />
            ))}
 
  </div>
    </div>
    </div>
    
  )
}
