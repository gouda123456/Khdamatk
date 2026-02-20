import React, { useState } from 'react';

function AddExperience() {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');

  const handleSave = () => {
    alert('Saved!');
    console.log({ title, description });
  };

  const handleClose = () => {
    window.history.back();
  };

  return (
    <div className="min-h-screen bg-[#e0e0e0] flex items-center justify-center p-4 font-sans">
      
      <div className="bg-white w-full max-w-[600px] p-[40px] rounded-[4px] shadow-sm">
        
        <div className="mb-[30px]">
          <h2 className="text-[26px] font-bold text-[#333] m-0">Add Experience</h2>
        </div>

        <div className="mb-[25px]">
          <label className="block text-[14px] text-[#888] mb-[5px]">
            Title
          </label>
          <input
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            placeholder="Title"
            className="w-full border-b border-[#ccc] py-[10px] text-[18px] outline-none focus:border-[#333] transition-colors"
          />
        </div>

        <div className="mb-[25px]">
          <label className="block text-[14px] text-[#888] mb-[8px]">
            description
          </label>
          <textarea
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            className="w-full bg-[#f2f2f2] p-[15px] h-[150px] rounded-[4px] text-[16px] outline-none resize-none border-none"
          />
        </div>

        <div className="flex justify-end gap-[30px] mt-[40px]">
          <button
            onClick={handleSave}
            className="bg-transparent border-none text-[18px] font-semibold cursor-pointer text-[#333] hover:opacity-70 transition-opacity"
          >
            Save
          </button>
          <button
            onClick={handleClose}
            className="bg-transparent border-none text-[18px] font-semibold cursor-pointer text-[#333] hover:opacity-70 transition-opacity"
          >
            Close
          </button>
        </div>

      </div>
    </div>
  );
}

export default AddExperience;