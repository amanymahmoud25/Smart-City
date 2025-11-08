using AutoMapper;
using Smart_City.Dtos;
using Smart_City.Models;
using Smart_City.Repositories;
using System;
using System.Collections.Generic;

namespace Smart_City.Managers
{
    public class UtilityIssueManager : IUtilityIssueManager
    {
        private readonly UtilityIssueRepository _repo;
        private readonly IMapper _mapper;

        public UtilityIssueManager(UtilityIssueRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public List<UtilityIssueDto> GetAll()
        {
            var list = _repo.GetAll();
            return _mapper.Map<List<UtilityIssueDto>>(list);
        }

        public UtilityIssueDto GetById(int id)
        {
            var issue = _repo.GetById(id);
            if (issue == null) return null;
            return _mapper.Map<UtilityIssueDto>(issue);
        }

        public List<UtilityIssueDto> GetByCitizenId(int citizenId)
        {
            var list = _repo.GetByCitizenId(citizenId);
            return _mapper.Map<List<UtilityIssueDto>>(list);
        }

        public UtilityIssueDto Create(UtilityIssueCreateDto dto, int citizenId)
        {
            if (dto == null) return null;

            var issue = new UtilityIssue
            {
                Type = dto.Type,
                Description = dto.Description,
                ReportDate = DateTime.Now,
                Status = "Pending",
                CitizenId = citizenId
            };

            var saved = _repo.Add(issue); 
            if (!saved) return null;


                 return _mapper.Map<UtilityIssueDto>(issue);
        }

        public UtilityIssueDto Update(UtilityIssueUpdateDto dto)
        {
            if (dto == null) return null;

            var issue = _repo.GetById(dto.Id);
            if (issue == null) return null;

            if (!string.IsNullOrEmpty(dto.Status))
                issue.Status = dto.Status;

            if (!string.IsNullOrEmpty(dto.Description))
                issue.Description = dto.Description;

            var updated = _repo.Update(issue);
            if (!updated) return null;

            return _mapper.Map<UtilityIssueDto>(issue);
        }

        public bool Delete(int id)
        {
            return _repo.Delete(id);
        }

        public bool MarkAsResolved(int id)
        {
            return _repo.MarkAsResolved(id);
        }

        public List<UtilityIssueDto> GetByType(UtilityIssueType type)
        {
            var list = _repo.GetByType(type);
            return _mapper.Map<List<UtilityIssueDto>>(list);
        }

        public List<UtilityIssueDto> GetByStatus(string status)
        {
            var list = _repo.GetByStatus(status);
            return _mapper.Map<List<UtilityIssueDto>>(list);
        }
    }
}
