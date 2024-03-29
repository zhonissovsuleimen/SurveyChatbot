﻿using Microsoft.EntityFrameworkCore;
using SurveyChatbot.Database;
using SurveyChatbot.Models;

namespace SurveyChatbot.Repositories;

public class SurveyRepository : IDataRepository<Survey>
{
    private readonly DatabaseContext _context;

    public SurveyRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Survey[]> GetAllAsync()
    {
        return await _context.Surveys.ToArrayAsync();
    }

    public async Task<Survey?> GetByIdAsync(long id)
    {
        var survey = await _context.Surveys.FindAsync(id);
        
        //Do not remove below line, as it seems to load all questions for survey.
        //Without it questions list will be empty.
        var questions = _context.Questions.Where(q => survey != null && q.Survey.Id == survey.Id).ToArray();
        return survey;
    }

    public async Task<Survey?> GetBySearchIdAsync(string searchId)
    {
        var survey = await _context.Surveys.FirstOrDefaultAsync(s => s.SearchId == searchId);

        //Do not remove below line, as it seems to load all questions for survey.
        //Without it questions list will be empty.
        var questions = _context.Questions.Where(q => survey != null && q.Survey.Id == survey.Id).ToArray();
        return survey;
    }

    public async Task AddAsync(Survey data)
    {
        await _context.Surveys.AddAsync(data);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ContainsAsync(Survey data)
    {
        return await _context.Surveys.ContainsAsync(data);
    }

    public void Add(Survey data)
    {
        _context.Surveys.Add(data);
        _context.SaveChanges();
    }

    public Survey[] GetAll()
    {
        return _context.Surveys.ToArray();
    }
}