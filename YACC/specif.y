%{
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#define YYDEBUG 1

double stiva[20];
int sp;

void push(double x)
{ stiva[sp++]=x; }

double pop()
{ return stiva[--sp]; }

int yylex() {}

int yyerror(char *s)
{
  printf("%s\n", s);
}

FILE *yyin;

%}

%token CNST
%token SYM

%token thus
%token is
%token known
%token as
%token shall
%token be
%token numeric
%token text
%token complex

%%
program: statementList
statementList: statement '.' | statement '.' sequentialStatement
sequentialStatement: statement | statement ';' sequentialStatement
statement: declarationStatement
declarationStatement: simpleDeclaration
simpleDeclaration: thus SYM is type
type: numeric | text
%%
main(int argc, char **argv)
{
  if(argc>1) yyin = fopen(argv[1], "r");
  if((argc>2)&&(!strcmp(argv[2],"-d"))) yydebug = 1;
  if(!yyparse()) fprintf(stderr,"\tO.K.\n");
}

