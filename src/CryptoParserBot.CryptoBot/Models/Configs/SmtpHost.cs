﻿namespace CryptoParserBot.CryptoBot.Models.Configs;

public sealed class SmtpHost
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}