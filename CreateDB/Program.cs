// See https://aka.ms/new-console-template for more information
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

SqlConnection con = new SqlConnection(
    "server=DESKTOP-A1JL32J;database=bankingDB;integrated security=true");
con.Open();

int usrNo = 0;
int accNo = 10000;
Random rnd = new Random(94632);

for (int i = 10001; i <= 11000; i++)
{
    // randomly generate number of accounts and which accounts each person has
    // each person should have a checkings with savings and loan optional
    // the i variable will be the userNumber
    usrNo = i;
    int numberOfAccounts = rnd.Next(1, 5);
    switch (numberOfAccounts)
    {
        case 1:
            // create checking account only
            SqlCommand cmd = new SqlCommand(
                "insert into ACCOUNTS (userNumber, accType) values(@usrNo, 'Checkings')", con);
            cmd.Parameters.AddWithValue("@usrNo", usrNo);
            cmd.ExecuteNonQuery();

            break;
        case 2:
            //create checking and savings
            SqlCommand cmd1 = new SqlCommand(
                "insert into ACCOUNTS (userNumber, accType) values(@usrNo, 'Checkings')", con);
            SqlCommand cmd2 = new SqlCommand(
                "insert into ACCOUNTS (userNumber, accType) values(@usrNo, 'Savings')", con);
            cmd1.Parameters.AddWithValue("@usrNo", usrNo);
            cmd2.Parameters.AddWithValue("@usrNo", usrNo);
            cmd1.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();

            break;
        case 3:
            //create checking and loan
            SqlCommand cmd3 = new SqlCommand(
                "insert into ACCOUNTS (userNumber, accType) values(@usrNo, 'Checkings')", con);
            SqlCommand cmd4 = new SqlCommand(
                "insert into ACCOUNTS (userNumber, accType) values(@usrNo, 'Loan')", con);
            cmd3.Parameters.AddWithValue("@usrNo", usrNo);
            cmd4.Parameters.AddWithValue("@usrNo", usrNo);
            cmd3.ExecuteNonQuery();
            cmd4.ExecuteNonQuery();

            break;
        case 4:
            //create checking, savings, and loan
            SqlCommand cmd5 = new SqlCommand(
                "insert into ACCOUNTS (userNumber, accType) values(@usrNo, 'Checkings')", con);
            SqlCommand cmd6 = new SqlCommand(
                "insert into ACCOUNTS (userNumber, accType) values(@usrNo, 'Savings')", con);
            SqlCommand cmd7 = new SqlCommand(
                "insert into ACCOUNTS (userNumber, accType) values(@usrNo, 'Loan')", con);
            cmd5.Parameters.AddWithValue("@usrNo", usrNo);
            cmd6.Parameters.AddWithValue("@usrNo", usrNo);
            cmd7.Parameters.AddWithValue("@usrNo", usrNo);
            cmd5.ExecuteNonQuery();
            cmd6.ExecuteNonQuery();
            cmd7.ExecuteNonQuery();


            break;
    }
    // Start transactions logic 

    int numberOfTransactions = rnd.Next(5, 15);
    string[] withdrawalDescriptionsC = { "Retail Store", "Groceries", "Online Purchase",
    "In Person Withdrawal"};
    string[] depositDescriptionsC = { "In Person Deposit", "Paycheck" };

    //all accounts have a checkings so this is run for all accounts
    //first transaction - initial deposit - update ACCOUNTS table
    double initialDeposit = (double)rnd.Next(20, 10000) + Math.Round(rnd.NextDouble(), 2);
    SqlCommand cmd0 = new SqlCommand(
        "update ACCOUNTS set accBalance=@initialDeposit where accNumber=@accNo", con);
    cmd0.Parameters.AddWithValue("@initialDeposit", initialDeposit);
    cmd0.Parameters.AddWithValue("@accNo", accNo);
    cmd0.ExecuteNonQuery();

    //first transaction - update TRANSACTIONS table
    SqlCommand cmd10 = new SqlCommand(
        "insert into TRANSACTIONS (accNumber, userNumber, transAmount, transDescription) values (@accNo, @usrNo, @initialDeposit, 'Initial Deposit')",
        con);
    cmd10.Parameters.AddWithValue("@accNo", accNo);
    cmd10.Parameters.AddWithValue("@usrNo", usrNo);
    cmd10.Parameters.AddWithValue("@initialDeposit", initialDeposit);
    cmd10.ExecuteNonQuery();

    double accBalance = initialDeposit;
    // Create transactions! yay....
    for (int a = 1; a <= numberOfTransactions; a++)
    {
        // 75% ish chance to be a withdrawal, 25% for deposits
        double transAmount = 0.0;
        if (rnd.Next(1, 5) != 4)
        {
            transAmount = -1 * Math.Round((double)rnd.Next(1, 1+(int)accBalance / 2) + rnd.NextDouble(), 2);
        }
        else
        {
            transAmount = Math.Round((double)rnd.Next(20, 10000) + rnd.NextDouble(), 2);
        }

        // update TRANSACTIONS table
        cmd0 = new SqlCommand(
        "insert into TRANSACTIONS (accNumber, userNumber, transAmount, transDescription) values (@accNo, @usrNo, @transAmount, @transReason)",
        con);
        cmd0.Parameters.AddWithValue("@accNo", accNo);
        cmd0.Parameters.AddWithValue("@usrNo", usrNo);
        cmd0.Parameters.AddWithValue("@transAmount", transAmount);
        if (transAmount >= 0)
        {
            cmd0.Parameters.AddWithValue("@transReason", depositDescriptionsC[rnd.Next(0, 2)]);
        }
        else
        {
            cmd0.Parameters.AddWithValue("@transReason", withdrawalDescriptionsC[rnd.Next(0, 4)]);
        }
        cmd0.ExecuteNonQuery();

        accBalance = accBalance + transAmount;
        // update ACCOUNTS table
        cmd10 = new SqlCommand(
            "update ACCOUNTS set accBalance=@newBalance where accNumber=@accNo", con);
        cmd10.Parameters.AddWithValue("@newBalance", accBalance);
        cmd10.Parameters.AddWithValue("@accNo", accNo);
        cmd10.ExecuteNonQuery();

    }

    switch (numberOfAccounts)
    {
        case 1:
            // checking transactions have already been added so increase the accNo and
            // go back to beginning to for loop
            accNo++;
            break;
        case 2:
            accNo++; //add one here because checking is done
            //do savings account
            string[] withdrawalDescriptionsS = { "Retail Store", "Groceries", "Online Purchase",
                "In Person Withdrawal"};
            string[] depositDescriptionsS = { "In Person Deposit", "Paycheck" };
            //first transaction - initial deposit - update ACCOUNTS table
            double initialDepositS = (double)rnd.Next(20, 10000) + Math.Round(rnd.NextDouble(), 2);
            SqlCommand cmd1 = new SqlCommand(
                "update ACCOUNTS set accBalance=@initialDeposit where accNumber=@accNo", con);
            cmd1.Parameters.AddWithValue("@initialDeposit", initialDepositS);
            cmd1.Parameters.AddWithValue("@accNo", accNo);
            cmd1.ExecuteNonQuery();

            //first transaction - update TRANSACTIONS table
            SqlCommand cmd2 = new SqlCommand(
                "insert into TRANSACTIONS (accNumber, userNumber, transAmount, transDescription) values (@accNo, @usrNo, @initialDeposit, 'Initial Deposit')",
                con);
            cmd2.Parameters.AddWithValue("@accNo", accNo);
            cmd2.Parameters.AddWithValue("@usrNo", usrNo);
            cmd2.Parameters.AddWithValue("@initialDeposit", initialDepositS);
            cmd2.ExecuteNonQuery();

            double accBalanceS = initialDepositS;
            // Create transactions! yay....
            for (int a = 1; a <= rnd.Next(5, 15); a++)
            {
                // 75% ish chance to be a withdrawal, 25% for deposits
                double transAmount = 0.0;
                if (rnd.Next(1, 5) != 4)
                {
                    transAmount = -1 * Math.Round((double)rnd.Next(1, 1+(int)accBalanceS / 4) + rnd.NextDouble(), 2);
                }
                else
                {
                    transAmount = Math.Round((double)rnd.Next(20, 10000) + rnd.NextDouble(), 2);
                }

                // update TRANSACTIONS table
                cmd1 = new SqlCommand(
                "insert into TRANSACTIONS (accNumber, userNumber, transAmount, transDescription) values (@accNo, @usrNo, @transAmount, @transReason)",
                con);
                cmd1.Parameters.AddWithValue("@accNo", accNo);
                cmd1.Parameters.AddWithValue("@usrNo", usrNo);
                cmd1.Parameters.AddWithValue("@transAmount", transAmount);
                if (transAmount >= 0)
                {
                    cmd1.Parameters.AddWithValue("@transReason", depositDescriptionsS[rnd.Next(0, 2)]);
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@transReason", withdrawalDescriptionsS[rnd.Next(0, 4)]);
                }
                cmd1.ExecuteNonQuery();

                accBalanceS = accBalanceS + transAmount;
                // update ACCOUNTS table
                cmd2 = new SqlCommand(
                    "update ACCOUNTS set accBalance=@newBalance where accNumber=@accNo", con);
                cmd2.Parameters.AddWithValue("@newBalance", accBalanceS);
                cmd2.Parameters.AddWithValue("@accNo", accNo);
                cmd2.ExecuteNonQuery();

            }

            accNo++;
            break;
        case 3:
            accNo++; //add one here because checking is done
            //do loan account
            //first transaction - initial deposit - update ACCOUNTS table
            initialDepositS = (double)rnd.Next(2000, 10000) + Math.Round(rnd.NextDouble(), 2);
            cmd1 = new SqlCommand(
                "update ACCOUNTS set accBalance=@initialDeposit where accNumber=@accNo", con);
            cmd1.Parameters.AddWithValue("@initialDeposit", initialDepositS);
            cmd1.Parameters.AddWithValue("@accNo", accNo);
            cmd1.ExecuteNonQuery();

            //first transaction - update TRANSACTIONS table
            cmd2 = new SqlCommand(
                "insert into TRANSACTIONS (accNumber, userNumber, transAmount, transDescription) values (@accNo, @usrNo, @initialDeposit, 'Initial Deposit')",
                con);
            cmd2.Parameters.AddWithValue("@accNo", accNo);
            cmd2.Parameters.AddWithValue("@usrNo", usrNo);
            cmd2.Parameters.AddWithValue("@initialDeposit", initialDepositS);
            cmd2.ExecuteNonQuery();

            accBalanceS = initialDepositS;
            // Create transactions! yay....
            for (int a = 1; a <= rnd.Next(5, 15); a++)
            {
                // 75% ish chance to be a payment, 25% for increase
                double transAmount = 0.0;
                if (rnd.Next(1, 5) != 4)
                {
                    transAmount = -1 * Math.Round((double)rnd.Next(1, 1+(int)accBalanceS / 4) + rnd.NextDouble(), 2);
                }
                else
                {
                    transAmount = Math.Round(accBalanceS*1.1, 2);
                }

                // update TRANSACTIONS table
                cmd1 = new SqlCommand(
                "insert into TRANSACTIONS (accNumber, userNumber, transAmount, transDescription) values (@accNo, @usrNo, @transAmount, @transReason)",
                con);
                cmd1.Parameters.AddWithValue("@accNo", accNo);
                cmd1.Parameters.AddWithValue("@usrNo", usrNo);
                cmd1.Parameters.AddWithValue("@transAmount", transAmount);
                if (transAmount >= 0)
                {
                    cmd1.Parameters.AddWithValue("@transReason", "Loan Increase/Interest");
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@transReason", "Loan Payment");
                }
                cmd1.ExecuteNonQuery();

                accBalanceS = accBalanceS + transAmount;
                // update ACCOUNTS table
                cmd2 = new SqlCommand(
                    "update ACCOUNTS set accBalance=@newBalance where accNumber=@accNo", con);
                cmd2.Parameters.AddWithValue("@newBalance", accBalanceS);
                cmd2.Parameters.AddWithValue("@accNo", accNo);
                cmd2.ExecuteNonQuery();

            }


            accNo++;
            break;
        case 4:
            accNo++; //add one here because checking is done
            //do the savings account here
            //do savings account
            string[] withdrawalDescriptionsS1 = { "Retail Store", "Groceries", "Online Purchase",
                "In Person Withdrawal"};
            string[] depositDescriptionsS1 = { "In Person Deposit", "Paycheck" };
            //first transaction - initial deposit - update ACCOUNTS table
            initialDepositS = (double)rnd.Next(20, 10000) + Math.Round(rnd.NextDouble(), 2);
            cmd1 = new SqlCommand(
                "update ACCOUNTS set accBalance=@initialDeposit where accNumber=@accNo", con);
            cmd1.Parameters.AddWithValue("@initialDeposit", initialDepositS);
            cmd1.Parameters.AddWithValue("@accNo", accNo);
            cmd1.ExecuteNonQuery();

            //first transaction - update TRANSACTIONS table
            cmd2 = new SqlCommand(
                "insert into TRANSACTIONS (accNumber, userNumber, transAmount, transDescription) values (@accNo, @usrNo, @initialDeposit, 'Initial Deposit')",
                con);
            cmd2.Parameters.AddWithValue("@accNo", accNo);
            cmd2.Parameters.AddWithValue("@usrNo", usrNo);
            cmd2.Parameters.AddWithValue("@initialDeposit", initialDepositS);
            cmd2.ExecuteNonQuery();

            accBalanceS = initialDepositS;
            // Create transactions! yay....
            for (int a = 1; a <= rnd.Next(5, 15); a++)
            {
                // 75% ish chance to be a withdrawal, 25% for deposits
                double transAmount = 0.0;
                if (rnd.Next(1, 5) != 4)
                {
                    transAmount = -1 * Math.Round((double)rnd.Next(1, 1 + (int)accBalanceS / 4) + rnd.NextDouble(), 2);
                }
                else
                {
                    transAmount = Math.Round((double)rnd.Next(20, 10000) + rnd.NextDouble(), 2);
                }

                // update TRANSACTIONS table
                cmd1 = new SqlCommand(
                "insert into TRANSACTIONS (accNumber, userNumber, transAmount, transDescription) values (@accNo, @usrNo, @transAmount, @transReason)",
                con);
                cmd1.Parameters.AddWithValue("@accNo", accNo);
                cmd1.Parameters.AddWithValue("@usrNo", usrNo);
                cmd1.Parameters.AddWithValue("@transAmount", transAmount);
                if (transAmount >= 0)
                {
                    cmd1.Parameters.AddWithValue("@transReason", depositDescriptionsS1[rnd.Next(0, 2)]);
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@transReason", withdrawalDescriptionsS1[rnd.Next(0, 4)]);
                }
                cmd1.ExecuteNonQuery();

                accBalanceS = accBalanceS + transAmount;
                // update ACCOUNTS table
                cmd2 = new SqlCommand(
                    "update ACCOUNTS set accBalance=@newBalance where accNumber=@accNo", con);
                cmd2.Parameters.AddWithValue("@newBalance", accBalanceS);
                cmd2.Parameters.AddWithValue("@accNo", accNo);
                cmd2.ExecuteNonQuery();

            }


            accNo++;
            //do the loan account here
            //first transaction - initial deposit - update ACCOUNTS table
            initialDepositS = (double)rnd.Next(2000, 10000) + Math.Round(rnd.NextDouble(), 2);
            cmd1 = new SqlCommand(
                "update ACCOUNTS set accBalance=@initialDeposit where accNumber=@accNo", con);
            cmd1.Parameters.AddWithValue("@initialDeposit", initialDepositS);
            cmd1.Parameters.AddWithValue("@accNo", accNo);
            cmd1.ExecuteNonQuery();

            //first transaction - update TRANSACTIONS table
            cmd2 = new SqlCommand(
                "insert into TRANSACTIONS (accNumber, userNumber, transAmount, transDescription) values (@accNo, @usrNo, @initialDeposit, 'Initial Deposit')",
                con);
            cmd2.Parameters.AddWithValue("@accNo", accNo);
            cmd2.Parameters.AddWithValue("@usrNo", usrNo);
            cmd2.Parameters.AddWithValue("@initialDeposit", initialDepositS);
            cmd2.ExecuteNonQuery();

            accBalanceS = initialDepositS;
            // Create transactions! yay....
            for (int a = 1; a <= rnd.Next(5, 15); a++)
            {
                // 75% ish chance to be a payment, 25% for increase
                double transAmount = 0.0;
                if (rnd.Next(1, 5) != 4)
                {
                    transAmount = -1 * Math.Round((double)rnd.Next(1, 1 + (int)accBalanceS / 4) + rnd.NextDouble(), 2);
                }
                else
                {
                    transAmount = Math.Round(accBalanceS * 1.1, 2);
                }

                // update TRANSACTIONS table
                cmd1 = new SqlCommand(
                "insert into TRANSACTIONS (accNumber, userNumber, transAmount, transDescription) values (@accNo, @usrNo, @transAmount, @transReason)",
                con);
                cmd1.Parameters.AddWithValue("@accNo", accNo);
                cmd1.Parameters.AddWithValue("@usrNo", usrNo);
                cmd1.Parameters.AddWithValue("@transAmount", transAmount);
                if (transAmount >= 0)
                {
                    cmd1.Parameters.AddWithValue("@transReason", "Loan Increase/Interest");
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@transReason", "Loan Payment");
                }
                cmd1.ExecuteNonQuery();

                accBalanceS = accBalanceS + transAmount;
                // update ACCOUNTS table
                cmd2 = new SqlCommand(
                    "update ACCOUNTS set accBalance=@newBalance where accNumber=@accNo", con);
                cmd2.Parameters.AddWithValue("@newBalance", accBalanceS);
                cmd2.Parameters.AddWithValue("@accNo", accNo);
                cmd2.ExecuteNonQuery();

            }
            
            accNo++; 
            break;
    }


    // generate account in the ACCOUNTS table
    // account number will be incremented everytime an account is created 
    // every account is created with $0 but transactions should be created immediately
    // so that the account number and user number is accessible for the sql insert

    // create 5-15 transactions for each account
    // Every first transaction should be 'Initial Deposit' with the starting amount
    // randomly generate how many transactions the account should have (5-15)
    // depending on account type, write out a list of Transaction Descriptions to attribute
    // Transaction amounts will be a random number with a minimum of $1 and a maximum of
    // accBalance for withdrawals and a minimum of $20 and a maximum of $10000 for deposits
    
}

con.Close();
