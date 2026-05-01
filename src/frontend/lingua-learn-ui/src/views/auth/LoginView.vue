<script setup lang="ts">
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import LoginForm from '@/features/auth/components/LoginForm.vue'
import type { LoginFormValues } from '@/features/auth/types'

function handleLogin(values: LoginFormValues) {
  void values
}
</script>

<template>
  <main class="min-h-screen bg-background text-foreground">
    <section class="grid min-h-screen lg:grid-cols-[1fr_30rem]">
      <div class="hidden bg-zinc-950 text-white lg:block">
        <div class="flex h-full flex-col justify-between px-12 py-10">
          <div class="text-lg font-semibold">Lingua Learn</div>
          <div class="max-w-lg space-y-5">
            <p class="text-4xl font-semibold leading-tight">Build a steadier language practice, one review at a time.</p>
            <p class="text-base leading-7 text-zinc-300">
              Keep focused sessions, review due cards, and track learning momentum from one quiet workspace.
            </p>
          </div>
          <p class="text-sm text-zinc-400">Adaptive flashcards for serious learners</p>
        </div>
      </div>

      <div class="flex min-h-screen items-center justify-center px-6 py-10">
        <Card class="w-full max-w-sm">
          <CardHeader>
            <CardDescription>Welcome back</CardDescription>
            <CardTitle class="text-3xl">Sign in to your account</CardTitle>
            <CardDescription>
              Continue to your learning dashboard.
            </CardDescription>
          </CardHeader>

          <CardContent>
            <LoginForm @submit="handleLogin" />
          </CardContent>

          <CardFooter class="justify-center">
            <p class="text-center text-sm text-muted-foreground">
              New to Lingua Learn?
              <a class="font-medium text-foreground underline-offset-4 hover:underline" href="/register">Create an account</a>
            </p>
          </CardFooter>
        </Card>
      </div>
    </section>
  </main>
</template>
