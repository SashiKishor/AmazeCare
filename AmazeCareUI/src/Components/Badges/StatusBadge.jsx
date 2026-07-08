import React from 'react';

function StatusBadge({ status }) {
  
  const baseClasses = "badge rounded-pill px-3 py-2 fw-semibold";

  switch (status) {
    case 'Requested':
      
      return <span className={`${baseClasses} bg-warning text-dark`}>Requested</span>;
      
    case 'Upcoming':
    case 'Scheduled':
    case 'Confirmed':
      return <span className={`${baseClasses} bg-primary text-white`}>{status}</span>;
      
    case 'Rescheduled':
      return <span className={`${baseClasses} bg-info text-dark`}>Rescheduled</span>;
      
    case 'Completed':
      
      return <span className={`${baseClasses} bg-success text-white`}>Completed</span>;
      
    case 'Cancelled':
      return <span className={`${baseClasses} bg-danger text-white`}>Cancelled</span>;
      
    default:
      return <span className={`${baseClasses} bg-secondary text-white`}>{status || 'Unknown'}</span>;
  }
}

export default StatusBadge;