interface Job {
  id: string;
  title: string;
  company: string;
  location: string;
  description: string;
  contract_time: string;
  redirect_url: string;
}

export async function generateJSON(data: Job){
    const jobData = {
        jobName: data.title + " at " + data.company,
        jobDescription: data.description,
    };

    const response = await fetch('/api/resume-api/Job/json', {
        method: 'POST',
        headers: {
        "Content-Type": "application/json",
        },
        body: JSON.stringify({
          type: "Resume",
          data: jobData
        }),
    })
    .then(res => res.json());

    if (response.error) {
        console.error("Error generating resume:", response.error);
        return;
    }

    const savedResumes = {
        id: data.id,
        resume: response,
    };

    let existingResumes = JSON.parse(localStorage.getItem("savedResumes") || "[]");
    existingResumes.push(savedResumes);
    localStorage.setItem("savedResumes", JSON.stringify(existingResumes));
}

export function generateResume(data: Job){
}